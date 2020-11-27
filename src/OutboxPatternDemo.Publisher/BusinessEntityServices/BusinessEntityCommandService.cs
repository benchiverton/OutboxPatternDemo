using System;
using OutboxPatternDemo.Publisher.BusinessEntityServices.Data;
using OutboxPatternDemo.Publisher.Contract.Events;
using OutboxPatternDemo.Publisher.Contract.Models;
using OutboxPatternDemo.Publisher.Infrastructure;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices
{
    public interface IBusinessEntityCommandService
    {
        StateDetail AddStateDetail(string businessEntityId, StateDetail detail, bool sendDuplicateEvent);
    }

    public class BusinessEntityCommandService : IBusinessEntityCommandService
    {
        private readonly BusinessEntityContext _stateDetailContext;
        private readonly IOutboxMessageBus _outboxMessageBus;

        public BusinessEntityCommandService(BusinessEntityContext stateDetailContext, IOutboxMessageBus outboxMessageBus)
        {
            _stateDetailContext = stateDetailContext;
            _outboxMessageBus = outboxMessageBus;
        }

        public StateDetail AddStateDetail(string businessEntityId, StateDetail detail, bool sendDuplicateEvent)
        {
            var dto = detail.ToStateDetailDto(businessEntityId);

            // begin DB transaction
            using var transaction = _stateDetailContext.Database.BeginTransaction();
            _outboxMessageBus.SetTransaction(transaction);

            // persist new state details to DB
            dto.TimeStampUtc = DateTime.UtcNow;
            _stateDetailContext.StateDetails.Add(dto);
            _stateDetailContext.SaveChanges();

            // publish message to outbox message bus
            var stateDetail = dto.ToStateDetail();
            _outboxMessageBus.Publish(nameof(StateUpdated), new StateUpdated
            {
                BusinessEntityId = businessEntityId,
                Details = stateDetail,
            }, sendDuplicateEvent);

            // commit transaction
            transaction.Commit();

            return stateDetail;
        }
    }
}
