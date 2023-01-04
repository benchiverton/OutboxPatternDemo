using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;

namespace OutboxPatternDemo.Subscriber.Handlers;

public class InconsistentAppointmentNotesEventHandler : IHandleMessages<AppointmentNotesAddedToMedicalRecord>
{
    private readonly ILogger<InconsistentAppointmentNotesEventHandler> _logger;
    private readonly IDuplicateChecker _duplicateChecker;

    public InconsistentAppointmentNotesEventHandler(ILogger<InconsistentAppointmentNotesEventHandler> logger, IDuplicateChecker duplicateChecker)
    {
        _logger = logger;
        _duplicateChecker = duplicateChecker;
    }

    /// <summary>
    /// If the business logic fails, then upon retrying a message it will be marked as duplicate.
    /// Therefore, this approach should be avoided where possible.
    /// </summary>
    public Task Handle(AppointmentNotesAddedToMedicalRecord message, IMessageHandlerContext context)
    {
        if (_duplicateChecker.IsDuplicate(message.Details.Id))
        {
            _logger.LogWarning($"{nameof(InconsistentAppointmentNotesEventHandler)} marked message with id: {message.Details.Id} as a duplicate. It will not be processed.");
            return Task.CompletedTask;
        }

        // business logic

        _logger.LogInformation($"{nameof(InconsistentAppointmentNotesEventHandler)} finished processing {nameof(AppointmentNotesAddedToMedicalRecord)} message with Id: {message.Details.Id}");

        return Task.CompletedTask;
    }
}
