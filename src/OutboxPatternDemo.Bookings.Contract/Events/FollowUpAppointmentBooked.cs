using System;
using System.Collections.Generic;

namespace OutboxPatternDemo.Bookings.Contract.Events;

public record FollowUpAppointmentBooked(string PatientName, DateTime AppointmentTimeUtc, List<Guid> AppointmentsToFollowUpOn);
