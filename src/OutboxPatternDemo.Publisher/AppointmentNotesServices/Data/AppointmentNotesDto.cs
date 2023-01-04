using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxPatternDemo.Publisher.AppointmentNotesServices.Data;

public class AppointmentNotesDto
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string PatientName { get; set; }
    public string Summary { get; set; }
    public string AppointmentType { get; set; }
    public bool RequiresFollowUpAppointment { get; set; }
    [Column(TypeName = "datetime2")]
    public DateTime AppointmentTimeUtc { get; set; }
}
