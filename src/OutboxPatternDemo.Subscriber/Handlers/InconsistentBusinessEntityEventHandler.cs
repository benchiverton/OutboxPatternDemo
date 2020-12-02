using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;

namespace OutboxPatternDemo.Subscriber.Handlers
{
    public class InconsistentBusinessEntityEventHandler : IHandleMessages<StateUpdated>
    {
        private readonly ILogger<InconsistentBusinessEntityEventHandler> _logger;
        private readonly IDuplicateChecker _duplicateChecker;

        public InconsistentBusinessEntityEventHandler(ILogger<InconsistentBusinessEntityEventHandler> logger, IDuplicateChecker duplicateChecker)
        {
            _logger = logger;
            _duplicateChecker = duplicateChecker;
        }

        /// <summary>
        /// If the business logic fails, then upon retrying a message it will be marked as duplicate.
        /// Therefore, this approach should be avoided where possible.
        /// </summary>
        public Task Handle(StateUpdated message, IMessageHandlerContext context)
        {
            if (_duplicateChecker.IsDuplicate(message.Details.Id))
            {
                _logger.LogWarning($"{nameof(StateUpdated)} message with id: {message.Details.Id} is a duplicate. It will not be processed.");
                return Task.CompletedTask;
            }

            // business logic

            _logger.LogInformation($"{nameof(InconsistentBusinessEntityEventHandler)} finished processing {nameof(StateUpdated)} message with Id: {message.Details.Id}");

            return Task.CompletedTask;
        }
    }
}
