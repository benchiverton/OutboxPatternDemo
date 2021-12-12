using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Contract.Events;

public record StateUpdated(string BusinessEntityId, StateDetail Details) : IEvent;
