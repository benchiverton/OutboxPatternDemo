using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;

namespace OutboxPatternDemo.Subscriber.Handlers;

public class IdempotentAppointmentNotesEventHandler : IHandleMessages<AppointmentNotesAddedToMedicalRecord>
{
    private readonly ILogger<IdempotentAppointmentNotesEventHandler> _logger;

    public IdempotentAppointmentNotesEventHandler(ILogger<IdempotentAppointmentNotesEventHandler> logger) => _logger = logger;

    /// <summary>
    /// All logic within this message handler should be idempotent.
    /// </summary>
    public Task Handle(AppointmentNotesAddedToMedicalRecord message, IMessageHandlerContext context)
    {
        // idempotent business logic

        _logger.LogInformation($"{nameof(IdempotentAppointmentNotesEventHandler)} finished processing {nameof(AppointmentNotesAddedToMedicalRecord)} message with Id: {message.Details.Id}");

        return Task.CompletedTask;
    }
}
