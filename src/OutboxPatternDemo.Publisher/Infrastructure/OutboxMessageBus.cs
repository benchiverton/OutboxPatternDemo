using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NServiceBus;

namespace OutboxPatternDemo.Publisher.Infrastructure
{
    public interface IOutboxMessageBus
    {
        void Publish(string eventType, IEvent message, bool sendDuplicate);
        void SetTransaction(IDbContextTransaction transaction);
    }

    public class OutboxMessageBus : IOutboxMessageBus
    {
        private readonly OutboxContext _outboxContext;

        public OutboxMessageBus(OutboxContext outboxContext) => _outboxContext = outboxContext;

        public void SetTransaction(IDbContextTransaction transaction) => _outboxContext.Database.UseTransaction(transaction.GetDbTransaction());

        public void Publish(string eventType, IEvent message, bool sendDuplicate)
        {
            _outboxContext.Messages.Add(new OutboxMessage
            {
                Type = eventType,
                Data = JsonConvert.SerializeObject(message),
                RequestedTimeUtc = DateTime.UtcNow,
                SendDuplicate = sendDuplicate
            });
            _outboxContext.SaveChanges();
        }
    }
}
