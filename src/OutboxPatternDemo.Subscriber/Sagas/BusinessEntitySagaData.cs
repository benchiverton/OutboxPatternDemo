using System;
using System.Collections.Generic;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Subscriber.Sagas;

public class BusinessEntitySagaData : ContainSagaData
{
    public string BusinessEntityId { get; set; }
    public Dictionary<Guid, StateDetail> StateDetails { get; set; } = new Dictionary<Guid, StateDetail>();
}
