using System.Linq;
using OutboxPatternDemo.MedicalRecords.Contract.Models;
using OutboxPatternDemo.MedicalRecords.MedicalRecords.Data;

namespace OutboxPatternDemo.MedicalRecords.MedicalRecords.Services;

public interface IMedicalRecordQueryService
{
    public MedicalRecord GetMedicalRecord(string patientName);
}

public class MedicalRecordQueryService : IMedicalRecordQueryService
{
    private readonly MedicalRecordContext _medicalRecordContext;

    public MedicalRecordQueryService(MedicalRecordContext medicalRecordContext) => _medicalRecordContext = medicalRecordContext;

    public MedicalRecord GetMedicalRecord(string patientName)
    {
        var appointmentNotes = _medicalRecordContext.AppointmentNotes
            .Where(sd => sd.PatientName == patientName)
            .Select(sd => sd.ToAppointmentNotes());

        return new MedicalRecord(patientName, appointmentNotes.ToList());
    }
}
