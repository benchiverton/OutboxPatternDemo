using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Bookings.Contract.Commands;
using OutboxPatternDemo.Bookings.Contract.Events;
using OutboxPatternDemo.MedicalRecords.Contract.Events;

namespace OutboxPatternDemo.Bookings.FollowUpAppointments;

public class FollowUpAppointmentSaga : Saga<FollowUpAppointmentSagaData>,
    IAmStartedByMessages<AppointmentNotesAddedToMedicalRecord>,
    IHandleMessages<BookFollowUpAppointment>
{
    private readonly ILogger<FollowUpAppointmentSaga> _logger;

    public FollowUpAppointmentSaga(ILogger<FollowUpAppointmentSaga> logger) => _logger = logger;

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<FollowUpAppointmentSagaData> mapper) =>
        mapper.MapSaga(saga => saga.PatientName)
            .ToMessage<AppointmentNotesAddedToMedicalRecord>(message => message.PatientName)
            .ToMessage<BookFollowUpAppointment>(message => message.PatientName);

    public Task Handle(AppointmentNotesAddedToMedicalRecord message, IMessageHandlerContext context)
    {
        if (message.Details.RequiresFollowUpAppointment && !Data.AppointmentsToFollowUpOn.Contains(message.Details.Id))
        {
            Data.AppointmentsToFollowUpOn.Add(message.Details.Id);
        }

        if (Data.FollowUpRequired())
        {
            _logger.LogInformation($"Follow up needed for {Data.PatientName} for the following appointments:\n{string.Join('\n', Data.AppointmentsToFollowUpOn)}");
        }
        else
        {
            MarkAsComplete();
        }

        return Task.CompletedTask;
    }

    public async Task Handle(BookFollowUpAppointment message, IMessageHandlerContext context)
    {
        await context.Publish(new FollowUpAppointmentBooked(
            Data.PatientName,
            message.AppointmentTimeUtc,
            Data.AppointmentsToFollowUpOn
            ));

        MarkAsComplete();
    }
}
