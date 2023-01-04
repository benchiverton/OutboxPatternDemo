using System;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.MedicalRecords.Data;

public static class AppointmentNotesMappers
{
    public static AppointmentNotesDto ToAppointmentNotesDto(this AppointmentNotes appointmentNotes, string patientName) => new AppointmentNotesDto
    {
        Id = appointmentNotes.Id,
        PatientName = patientName,
        AppointmentType = appointmentNotes.AppointmentType.ToString(),
        Summary = appointmentNotes.Summary,
        RequiresFollowUpAppointment = appointmentNotes.RequiresFollowUpAppointment,
        AppointmentTimeUtc = appointmentNotes.AppointmentTimeUtc
    };

    public static AppointmentNotes ToAppointmentNotes(this AppointmentNotesDto appointmentNotesDto)
        => new AppointmentNotes(
            appointmentNotesDto.Id,
            Enum.Parse<AppointmentType>(appointmentNotesDto.AppointmentType),
            appointmentNotesDto.Summary,
            appointmentNotesDto.RequiresFollowUpAppointment,
            appointmentNotesDto.AppointmentTimeUtc
            );
}
