using System;
using System.Collections.Generic;

namespace OutboxPatternDemo.Publisher.Contract.Models;

public record MedicalRecord
{
    public MedicalRecord(string id, List<AppointmentNotes> stateDetails)
    {
        PatientName = id;
        HistoricAppointmentNotes = new List<AppointmentNotes>();
        foreach (var stateDetail in stateDetails)
        {
            HistoricAppointmentNotes.Add(stateDetail);
            if (LastAppointmentUtc < stateDetail.AppointmentTimeUtc)
            {
                LatestSummary = stateDetail.Summary;
                LastAppointmentUtc = stateDetail.AppointmentTimeUtc;
            }
        }
    }

    public string PatientName { get; }
    public string LatestSummary { get; }
    public DateTime LastAppointmentUtc { get; }
    public List<AppointmentNotes> HistoricAppointmentNotes { get; }
}
