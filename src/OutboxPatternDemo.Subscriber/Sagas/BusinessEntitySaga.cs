using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Events;

namespace OutboxPatternDemo.Subscriber.Sagas
{
    public class BusinessEntitySaga : Saga<BusinessEntitySagaData>,
        IAmStartedByMessages<StateUpdated>
    {
        private readonly ILogger<BusinessEntitySaga> _logger;

        public BusinessEntitySaga(ILogger<BusinessEntitySaga> logger) => _logger = logger;

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BusinessEntitySagaData> mapper) =>
            mapper.ConfigureMapping<StateUpdated>(message => message.BusinessEntityId)
                .ToSaga(sagaData => sagaData.BusinessEntityId);

        public Task Handle(StateUpdated message, IMessageHandlerContext context)
        {
            if (Data.StateDetails.ContainsKey(message.Details.Id))
            {
                // duplicate - don't process
                return Task.CompletedTask;
            }

            Data.StateDetails.Add(message.Details.Id, message.Details);

            // business logic

            _logger.LogInformation($"{nameof(BusinessEntitySaga)} finished processing {nameof(StateUpdated)} message with Id: {message.Details.Id}");

            return Task.CompletedTask;
        }
    }
}
