using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;

namespace OutboxPatternDemo.Subscriber.Handlers
{
    public class IdempotentBusinessEntityEventHandler : IHandleMessages<StateUpdated>
    {
        private readonly ILogger<IdempotentBusinessEntityEventHandler> _logger;

        public IdempotentBusinessEntityEventHandler(ILogger<IdempotentBusinessEntityEventHandler> logger) => _logger = logger;

        /// <summary>
        /// All logic within this message handler should be idempotent.
        /// </summary>
        public Task Handle(StateUpdated message, IMessageHandlerContext context)
        {
            // idempotent business logic

            _logger.LogInformation($"{nameof(IdempotentBusinessEntityEventHandler)} finished processing {nameof(StateUpdated)} message with Id: {message.Details.Id}");

            return Task.CompletedTask;
        }
    }
}
