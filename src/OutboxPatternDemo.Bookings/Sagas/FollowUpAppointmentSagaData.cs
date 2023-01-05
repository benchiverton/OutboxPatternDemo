using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using OutboxPatternDemo.MedicalRecords.Contract.Models;

namespace OutboxPatternDemo.Bookings.Sagas;

public class FollowUpAppointmentSagaData : ContainSagaData
{
    public string PatientName { get; set; }
    public Dictionary<Guid, AppointmentNotes> AppointmentsRequiringFollowUps { get; set; } = new Dictionary<Guid, AppointmentNotes>();

    public bool FollowUpRequired() => AppointmentsRequiringFollowUps.Any();
}
