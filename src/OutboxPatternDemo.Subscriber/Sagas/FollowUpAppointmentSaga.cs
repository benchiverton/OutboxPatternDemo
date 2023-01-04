using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;

namespace OutboxPatternDemo.Subscriber.Sagas;

public class FollowUpAppointmentSaga : Saga<FollowUpAppointmentSagaData>,
    IAmStartedByMessages<AppointmentNotesAddedToMedicalRecord>
{
    private readonly ILogger<FollowUpAppointmentSaga> _logger;

    public FollowUpAppointmentSaga(ILogger<FollowUpAppointmentSaga> logger) => _logger = logger;

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<FollowUpAppointmentSagaData> mapper) =>
        mapper.ConfigureMapping<AppointmentNotesAddedToMedicalRecord>(message => message.PatientName)
            .ToSaga(sagaData => sagaData.PatientName);

    public Task Handle(AppointmentNotesAddedToMedicalRecord message, IMessageHandlerContext context)
    {
        if (message.Details.RequiresFollowUpAppointment && !Data.AppointmentsRequiringFollowUps.ContainsKey(message.Details.Id))
        {
            Data.AppointmentsRequiringFollowUps.Add(message.Details.Id, message.Details);
        }

        if (Data.FollowUpRequired())
        {
            _logger.LogInformation($"Follow up needed for {Data.PatientName} for the following appointments:\n{string.Join('\n', Data.AppointmentsRequiringFollowUps)}\n");
        }
        else
        {
            MarkAsComplete();
        }

        return Task.CompletedTask;
    }
}
