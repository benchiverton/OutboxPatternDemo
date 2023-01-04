using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Contract.Events;

public record AppointmentNotesAddedToMedicalRecord(string PatientName, AppointmentNotes Details) : IEvent;
