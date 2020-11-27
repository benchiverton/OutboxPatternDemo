using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;

namespace OutboxPatternDemo.Subscriber.Handlers
{
    public class BusinessEntityEventHandler : IHandleMessages<StateUpdated>
    {
        private readonly ILogger<BusinessEntityEventHandler> _logger;
        private readonly IDuplicateChecker _duplicateChecker;

        public BusinessEntityEventHandler(ILogger<BusinessEntityEventHandler> logger, IDuplicateChecker duplicateChecker)
        {
            _logger = logger;
            _duplicateChecker = duplicateChecker;
        }

        public Task Handle(StateUpdated message, IMessageHandlerContext context)
        {
            if (_duplicateChecker.IsDuplicate(message.Details.Id))
            {
                _logger.LogWarning($"{nameof(StateUpdated)} message with id: {message.Details.Id} is a duplicate. It will not be processed.");
                return Task.CompletedTask;
            }

            // continue processing message
            _logger.LogInformation($"Finished processing {nameof(StateUpdated)} message with Id: {message.Details.Id}");

            return Task.CompletedTask;
        }
    }
}
