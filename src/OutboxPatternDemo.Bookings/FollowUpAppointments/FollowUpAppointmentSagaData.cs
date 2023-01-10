using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using OutboxPatternDemo.MedicalRecords.Contract.Models;

namespace OutboxPatternDemo.Bookings.FollowUpAppointments;

public class FollowUpAppointmentSagaData : ContainSagaData
{
    public string PatientName { get; set; }
    public List<Guid> AppointmentsToFollowUpOn { get; set; } = new List<Guid>();

    public bool FollowUpRequired() => AppointmentsToFollowUpOn.Any();
}
