using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OutboxPatternDemo.Publisher.BusinessEntityServices;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessEntityController : ControllerBase
    {
        private readonly ILogger<BusinessEntityController> _logger;
        private readonly IBusinessEntityCommandService _commandService;
        private readonly IBusinessEntityQueryService _queryService;

        public BusinessEntityController(ILogger<BusinessEntityController> logger, IBusinessEntityCommandService commandService, IBusinessEntityQueryService queryService)
        {
            _logger = logger;
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpPost("updatestate/{businessEntityId}")]
        public StateDetail UpdateState(string businessEntityId, [FromBody] StateDetail details)
        {
            _logger.LogInformation("");
            return _commandService.AddStateDetail(businessEntityId, details, false);
        }

        [HttpPost("updatestateandsendduplicate/{businessEntityId}")]
        public StateDetail UpdateStateAndSendDuplicate(string businessEntityId, [FromBody] StateDetail details)
        {
            _logger.LogInformation("");
            return _commandService.AddStateDetail(businessEntityId, details, true);
        }

        [HttpGet("businessentity/{businessEntityId}")]
        public BusinessEntity GetBusinessEntityById(string businessEntityId)
        {
            _logger.LogInformation("");
            return _queryService.GetBusinessEntityById(businessEntityId);
        }
    }
}
