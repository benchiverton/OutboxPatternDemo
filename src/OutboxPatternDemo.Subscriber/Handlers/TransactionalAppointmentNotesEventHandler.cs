using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;

namespace OutboxPatternDemo.Subscriber.Handlers;

public class TransactionalAppointmentNotesEventHandler : IHandleMessages<AppointmentNotesAddedToMedicalRecord>
{
    private readonly ILogger<TransactionalAppointmentNotesEventHandler> _logger;
    private readonly ITransactionalDuplicateChecker _duplicateChecker;

    public TransactionalAppointmentNotesEventHandler(ILogger<TransactionalAppointmentNotesEventHandler> logger, ITransactionalDuplicateChecker duplicateChecker)
    {
        _logger = logger;
        _duplicateChecker = duplicateChecker;
    }

    public Task Handle(AppointmentNotesAddedToMedicalRecord message, IMessageHandlerContext context)
    {
        var sqlStorageSession = context.SynchronizedStorageSession.SqlPersistenceSession();

        if (_duplicateChecker.IsDuplicateTransactional(message.Details.Id, sqlStorageSession))
        {
            _logger.LogWarning($"{nameof(TransactionalAppointmentNotesEventHandler)} marked message with id: {message.Details.Id} as a duplicate. It will not be processed.");
            return Task.CompletedTask;
        }

        // business logic using transaction

        _logger.LogInformation($"{nameof(TransactionalAppointmentNotesEventHandler)} finished processing {nameof(AppointmentNotesAddedToMedicalRecord)} message with Id: {message.Details.Id}");

        return Task.CompletedTask;
    }
}
