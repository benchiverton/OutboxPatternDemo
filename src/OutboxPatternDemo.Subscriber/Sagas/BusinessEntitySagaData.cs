using System;
using System.Collections.Generic;
using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Subscriber.Sagas
{
    public class BusinessEntitySagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public string BusinessEntityId { get; set; }
        public Dictionary<int, StateDetail> StateDetails { get; set; } = new Dictionary<int, StateDetail>();
    }
}
