using System;

namespace OutboxPatternDemo.Bookings.Contract.Commands;

public record BookFollowUpAppointment(string PatientName, DateTime AppointmentTimeUtc);
