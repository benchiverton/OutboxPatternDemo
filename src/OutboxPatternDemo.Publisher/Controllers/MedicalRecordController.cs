using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutboxPatternDemo.Publisher.AppointmentNotesServices;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicalRecordController : ControllerBase
{
    private readonly ILogger<MedicalRecordController> _logger;
    private readonly IMedicalRecordCommandService _commandService;
    private readonly IMedicalRecordQueryService _queryService;

    public MedicalRecordController(ILogger<MedicalRecordController> logger, IMedicalRecordCommandService commandService, IMedicalRecordQueryService queryService)
    {
        _logger = logger;
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost("addappointmentnotesusingcustomoutbox/{patientName}")]
    public async Task<IActionResult> UpdateStateUsingCustomOutbox(string patientName, [FromBody] AppointmentNotes details)
    {
        await _commandService.AddAppointmentNotesUsingCustomOutbox(patientName, details);
        return Ok(details);
    }

    [HttpPost("addappointmentnotesusingnservicebusoutbox/{patientName}")]
    public async Task<IActionResult> UpdateStateUsingNServiceBusOutbox(string patientName, [FromBody] AppointmentNotes details)
    {
        await _commandService.AddAppointmentNotesUsingNServiceBusOutbox(patientName, details);
        return Ok(details);
    }

    [HttpGet("businessentity/{patientName}")]
    public MedicalRecord GetBusinessEntityById(string patientName) => _queryService.GetMedicalRecord(patientName);
}
