using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Publisher.MedicalRecords.Services;

namespace OutboxPatternDemo.Publisher.MedicalRecords.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly ILogger<MedicalRecordsController> _logger;
    private readonly IMedicalRecordCommandService _commandService;
    private readonly IMedicalRecordQueryService _queryService;

    public MedicalRecordsController(ILogger<MedicalRecordsController> logger, IMedicalRecordCommandService commandService, IMedicalRecordQueryService queryService)
    {
        _logger = logger;
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPut("{patientName}/appointmentnotes/customoutbox")]
    public async Task<IActionResult> AddAppointmentNotesUsingCustomOutbox(string patientName, [FromBody] AppointmentNotes details)
    {
        await _commandService.AddAppointmentNotesUsingCustomOutbox(patientName, details);
        return Ok(details);
    }

    [HttpPut("{patientName}/appointmentnotes/nservicebusoutbox")]
    public async Task<IActionResult> AddAppointmentNotesUsingNServiceBusOutbox(string patientName, [FromBody] AppointmentNotes details)
    {
        await _commandService.AddAppointmentNotesUsingNServiceBusOutbox(patientName, details);
        return Ok(details);
    }

    [HttpGet("{patientName}")]
    public MedicalRecord GetMedicalRecord(string patientName) => _queryService.GetMedicalRecord(patientName);
}
