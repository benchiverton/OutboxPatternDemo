using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;

namespace OutboxPatternDemo.Subscriber.Handlers;

public class TransactionalBusinessEntityEventHandler : IHandleMessages<StateUpdated>
{
    private readonly ILogger<TransactionalBusinessEntityEventHandler> _logger;
    private readonly IDuplicateChecker _duplicateChecker;

    public TransactionalBusinessEntityEventHandler(ILogger<TransactionalBusinessEntityEventHandler> logger, IDuplicateChecker duplicateChecker)
    {
        _logger = logger;
        _duplicateChecker = duplicateChecker;
    }

    public Task Handle(StateUpdated message, IMessageHandlerContext context)
    {
        var sqlStorageSession = context.SynchronizedStorageSession.SqlPersistenceSession();

        if (_duplicateChecker.IsDuplicateTransactional(message.Details.Id, sqlStorageSession))
        {
            _logger.LogWarning($"{nameof(TransactionalBusinessEntityEventHandler)} marked message with id: {message.Details.Id} as a duplicate. It will not be processed.");
            return Task.CompletedTask;
        }

        // business logic using transaction

        _logger.LogInformation($"{nameof(IdempotentBusinessEntityEventHandler)} finished processing {nameof(StateUpdated)} message with Id: {message.Details.Id}");

        return Task.CompletedTask;
    }
}
