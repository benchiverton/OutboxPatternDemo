using System;
using System.Collections.Generic;

namespace OutboxPatternDemo.MedicalRecords.Contract.Models;

public record MedicalRecord
{
    public MedicalRecord(string id, List<AppointmentNotes> appointmentNotes)
    {
        PatientName = id;
        HistoricAppointmentNotes = new List<AppointmentNotes>();
        foreach (var appointmentNote in appointmentNotes)
        {
            HistoricAppointmentNotes.Add(appointmentNote);
            if (LastAppointmentUtc < appointmentNote.AppointmentTimeUtc)
            {
                LatestSummary = appointmentNote.Summary;
                LastAppointmentUtc = appointmentNote.AppointmentTimeUtc;
            }
        }
    }

    public string PatientName { get; }
    public string LatestSummary { get; }
    public DateTime LastAppointmentUtc { get; }
    public List<AppointmentNotes> HistoricAppointmentNotes { get; }
}
