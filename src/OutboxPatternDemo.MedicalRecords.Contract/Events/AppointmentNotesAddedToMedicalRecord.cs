using NServiceBus;
using OutboxPatternDemo.MedicalRecords.Contract.Models;

namespace OutboxPatternDemo.MedicalRecords.Contract.Events;

public record AppointmentNotesAddedToMedicalRecord(string PatientName, AppointmentNotes Details) : IEvent;
