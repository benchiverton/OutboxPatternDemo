using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Contract.Commands;

public record AddAppointmentNotesToMedicalRecord(string PatientName, AppointmentNotes Details) : ICommand;
