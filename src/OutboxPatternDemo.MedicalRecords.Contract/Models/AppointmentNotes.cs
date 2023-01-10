using System;

namespace OutboxPatternDemo.MedicalRecords.Contract.Models;

public record AppointmentNotes(Guid Id, AppointmentType AppointmentType, string Summary, bool RequiresFollowUpAppointment, DateTime AppointmentTimeUtc)
{
    public Guid Id { get; init; } = Id == default ? Guid.NewGuid() : Id;
};
