using System.Linq;
using OutboxPatternDemo.Publisher.AppointmentNotesServices.Data;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.AppointmentNotesServices;

public interface IMedicalRecordQueryService
{
    public MedicalRecord GetMedicalRecord(string patientName);
}

public class MedicalRecordQueryService : IMedicalRecordQueryService
{
    private readonly MedicalRecordContext _stateDetailContext;

    public MedicalRecordQueryService(MedicalRecordContext stateDetailContext) => _stateDetailContext = stateDetailContext;

    public MedicalRecord GetMedicalRecord(string patientName)
    {
        var appointmentNotes = _stateDetailContext.AppointmentNotes
            .Where(sd => sd.PatientName == patientName)
            .Select(sd => sd.ToAppointmentNotes());

        return new MedicalRecord(patientName, appointmentNotes.ToList());
    }
}
