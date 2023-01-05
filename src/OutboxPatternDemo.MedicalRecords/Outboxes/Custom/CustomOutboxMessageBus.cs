using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NServiceBus;

namespace OutboxPatternDemo.MedicalRecords.Outboxes.Custom;

public interface IOutboxMessageBus
{
    void Publish(string eventType, IEvent message);
    void SetTransaction(IDbContextTransaction transaction);
}

public class CustomOutboxMessageBus : IOutboxMessageBus
{
    private readonly CustomOutboxContext _outboxContext;

    public CustomOutboxMessageBus(CustomOutboxContext outboxContext) => _outboxContext = outboxContext;

    public void SetTransaction(IDbContextTransaction transaction) => _outboxContext.Database.UseTransaction(transaction.GetDbTransaction());

    public void Publish(string eventType, IEvent message)
    {
        _outboxContext.Messages.Add(new CustomOutboxMessage
        {
            Type = eventType,
            Data = JsonConvert.SerializeObject(message),
            RequestedTimeUtc = DateTime.UtcNow
        });
        _outboxContext.SaveChanges();
    }
}