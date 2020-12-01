using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using OutboxPatternDemo.Publisher.BusinessEntityServices.Data;
using OutboxPatternDemo.Publisher.Contract.Commands;
using OutboxPatternDemo.Publisher.Contract.Events;

namespace OutboxPatternDemo.Publisher.NServiceBusOutbox
{
    public class NServiceBusOutboxHandler : IHandleMessages<UpdateState>
    {
        private readonly BusinessEntityContext _stateDetailContext;

        public NServiceBusOutboxHandler(BusinessEntityContext stateDetailContext) => _stateDetailContext = stateDetailContext;

        public async Task Handle(UpdateState message, IMessageHandlerContext context)
        {
            var persistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();

            // this can be auto-configured when setting up endpoint (.UseNServiceBus)
            _stateDetailContext.Database.SetDbConnection(persistenceSession.Connection);
            await _stateDetailContext.Database.UseTransactionAsync(persistenceSession.Transaction);

            var dto = message.Details.ToStateDetailDto(message.BusinessEntityId);

            dto.TimeStampUtc = DateTime.UtcNow;
            dto = (await _stateDetailContext.StateDetails.AddAsync(dto)).Entity;
            await _stateDetailContext.SaveChangesAsync();

            await context.Publish(new StateUpdated
            {
                BusinessEntityId = message.BusinessEntityId,
                Details = dto.ToStateDetail()
            });
        }
    }
}
