using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Contract.Events
{
    public class StateUpdated : IEvent
    {
        public string BusinessEntityId { get; set; }
        public StateDetail Details { get; set; }
    }
}
