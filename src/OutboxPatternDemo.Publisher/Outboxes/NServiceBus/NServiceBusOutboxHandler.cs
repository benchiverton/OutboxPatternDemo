using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Commands;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.MedicalRecords.Data;

namespace OutboxPatternDemo.Publisher.Outboxes.NServiceBus;

public class NServiceBusOutboxHandler : IHandleMessages<AddAppointmentNotesToMedicalRecord>
{
    private readonly MedicalRecordContext _medicalRecordContext;

    public NServiceBusOutboxHandler(MedicalRecordContext medicalRecordContext) => _medicalRecordContext = medicalRecordContext;

    public async Task Handle(AddAppointmentNotesToMedicalRecord message, IMessageHandlerContext context)
    {
        var persistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();

        // this can be auto-configured when setting up endpoint (.UseNServiceBus)
        _medicalRecordContext.Database.SetDbConnection(persistenceSession.Connection);
        await _medicalRecordContext.Database.UseTransactionAsync(persistenceSession.Transaction, context.CancellationToken);

        var dto = message.Details.ToAppointmentNotesDto(message.PatientName);

        dto.AppointmentTimeUtc = DateTime.UtcNow;
        dto = (await _medicalRecordContext.AppointmentNotes.AddAsync(dto, context.CancellationToken)).Entity;
        await _medicalRecordContext.SaveChangesAsync(context.CancellationToken);

        await context.Publish(new AppointmentNotesAddedToMedicalRecord(message.PatientName, dto.ToAppointmentNotes()));
    }
}
