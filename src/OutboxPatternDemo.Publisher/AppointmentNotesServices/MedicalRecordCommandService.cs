using System;
using System.Threading.Tasks;
using NServiceBus;
using OutboxPatternDemo.Publisher.AppointmentNotesServices.Data;
using OutboxPatternDemo.Publisher.Contract.Commands;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Publisher.CustomOutbox;

namespace OutboxPatternDemo.Publisher.AppointmentNotesServices;

public interface IMedicalRecordCommandService
{
    Task AddAppointmentNotesUsingCustomOutbox(string patientName, AppointmentNotes detail);
    Task AddAppointmentNotesUsingNServiceBusOutbox(string patientName, AppointmentNotes detail);
}

public class MedicalRecordCommandService : IMedicalRecordCommandService
{
    private readonly IMessageSession _messageSession;
    private readonly MedicalRecordContext _stateDetailContext;
    private readonly IOutboxMessageBus _outboxMessageBus;

    public MedicalRecordCommandService(IMessageSession messageSession, MedicalRecordContext stateDetailContext, IOutboxMessageBus outboxMessageBus)
    {
        _messageSession = messageSession;
        _stateDetailContext = stateDetailContext;
        _outboxMessageBus = outboxMessageBus;
    }

    public async Task AddAppointmentNotesUsingCustomOutbox(string patientName, AppointmentNotes detail)
    {
        var dto = detail.ToAppointmentNotesDto(patientName);

        // begin DB transaction
        await using var transaction = await _stateDetailContext.Database.BeginTransactionAsync();
        _outboxMessageBus.SetTransaction(transaction);

        // persist new state details to DB
        dto.AppointmentTimeUtc = DateTime.UtcNow;
        dto = (await _stateDetailContext.AppointmentNotes.AddAsync(dto)).Entity;
        await _stateDetailContext.SaveChangesAsync();

        // publish message to outbox message bus
        var stateDetail = dto.ToAppointmentNotes();
        _outboxMessageBus.Publish(nameof(AppointmentNotesAddedToMedicalRecord), new AppointmentNotesAddedToMedicalRecord(dto.PatientName, stateDetail));

        // commit transaction
        await transaction.CommitAsync();
    }

    // The outbox works only in an NServiceBus message handler
    public async Task AddAppointmentNotesUsingNServiceBusOutbox(string patientName, AppointmentNotes detail)
        => await _messageSession.SendLocal(new AddAppointmentNotesToMedicalRecord(patientName, detail));
}
