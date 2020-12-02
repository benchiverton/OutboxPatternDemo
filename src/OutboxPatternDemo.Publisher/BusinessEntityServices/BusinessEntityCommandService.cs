using System;
using System.Threading.Tasks;
using NServiceBus;
using OutboxPatternDemo.Publisher.BusinessEntityServices.Data;
using OutboxPatternDemo.Publisher.Contract.Commands;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Publisher.CustomOutbox;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices
{
    public interface IBusinessEntityCommandService
    {
        Task AddStateDetailUsingCustomOutbox(string businessEntityId, StateDetail detail);
        Task AddStateDetailUsingNServiceBusOutbox(string businessEntityId, StateDetail detail);
    }

    public class BusinessEntityCommandService : IBusinessEntityCommandService
    {
        private readonly IMessageSession _messageSession;
        private readonly BusinessEntityContext _stateDetailContext;
        private readonly IOutboxMessageBus _outboxMessageBus;

        public BusinessEntityCommandService(IMessageSession messageSession, BusinessEntityContext stateDetailContext, IOutboxMessageBus outboxMessageBus)
        {
            _messageSession = messageSession;
            _stateDetailContext = stateDetailContext;
            _outboxMessageBus = outboxMessageBus;
        }

        public async Task AddStateDetailUsingCustomOutbox(string businessEntityId, StateDetail detail)
        {
            var dto = detail.ToStateDetailDto(businessEntityId);

            // begin DB transaction
            await using var transaction = await _stateDetailContext.Database.BeginTransactionAsync();
            _outboxMessageBus.SetTransaction(transaction);

            // persist new state details to DB
            dto.TimeStampUtc = DateTime.UtcNow;
            dto = (await _stateDetailContext.StateDetails.AddAsync(dto)).Entity;
            await _stateDetailContext.SaveChangesAsync();

            // publish message to outbox message bus
            var stateDetail = dto.ToStateDetail();
            _outboxMessageBus.Publish(nameof(StateUpdated), new StateUpdated
            {
                BusinessEntityId = dto.BusinessEntityId,
                Details = stateDetail
            });

            // commit transaction
            await transaction.CommitAsync();
        }

        // The outbox works only in an NServiceBus message handler
        public async Task AddStateDetailUsingNServiceBusOutbox(string businessEntityId, StateDetail detail)
            => await _messageSession.SendLocal(new UpdateState
            {
                BusinessEntityId = businessEntityId,
                Details = detail
            });
    }
}
