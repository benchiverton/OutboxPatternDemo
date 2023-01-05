using System;
using System.Threading.Tasks;
using NServiceBus;
using OutboxPatternDemo.MedicalRecords.Contract.Commands;
using OutboxPatternDemo.MedicalRecords.Contract.Events;
using OutboxPatternDemo.MedicalRecords.Contract.Models;
using OutboxPatternDemo.MedicalRecords.MedicalRecords.Data;
using OutboxPatternDemo.MedicalRecords.Outboxes.Custom;

namespace OutboxPatternDemo.MedicalRecords.MedicalRecords.Services;

public interface IMedicalRecordCommandService
{
    Task AddAppointmentNotesUsingCustomOutbox(string patientName, AppointmentNotes detail);
    Task AddAppointmentNotesUsingNServiceBusOutbox(string patientName, AppointmentNotes detail);
}

public class MedicalRecordCommandService : IMedicalRecordCommandService
{
    private readonly IMessageSession _messageSession;
    private readonly MedicalRecordContext _medicalRecordContext;
    private readonly IOutboxMessageBus _outboxMessageBus;

    public MedicalRecordCommandService(IMessageSession messageSession, MedicalRecordContext medicalRecordContext, IOutboxMessageBus outboxMessageBus)
    {
        _messageSession = messageSession;
        _medicalRecordContext = medicalRecordContext;
        _outboxMessageBus = outboxMessageBus;
    }

    public async Task AddAppointmentNotesUsingCustomOutbox(string patientName, AppointmentNotes detail)
    {
        var dto = detail.ToAppointmentNotesDto(patientName);

        // begin DB transaction
        await using var transaction = await _medicalRecordContext.Database.BeginTransactionAsync();
        _outboxMessageBus.SetTransaction(transaction);

        // persist new appointment notes details to DB
        dto.AppointmentTimeUtc = DateTime.UtcNow;
        dto = (await _medicalRecordContext.AppointmentNotes.AddAsync(dto)).Entity;
        await _medicalRecordContext.SaveChangesAsync();

        // publish message to outbox message bus
        var appointmentNotes = dto.ToAppointmentNotes();
        _outboxMessageBus.Publish(nameof(AppointmentNotesAddedToMedicalRecord), new AppointmentNotesAddedToMedicalRecord(dto.PatientName, appointmentNotes));

        // commit transaction
        await transaction.CommitAsync();
    }

    // The outbox works only in an NServiceBus message handler
    public async Task AddAppointmentNotesUsingNServiceBusOutbox(string patientName, AppointmentNotes detail)
        => await _messageSession.SendLocal(new AddAppointmentNotesToMedicalRecord(patientName, detail));
}
