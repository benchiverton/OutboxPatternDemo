using NServiceBus;
using OutboxPatternDemo.MedicalRecords.Contract.Models;

namespace OutboxPatternDemo.MedicalRecords.Contract.Commands;

public record AddAppointmentNotesToMedicalRecord(string PatientName, AppointmentNotes Details) : ICommand;
