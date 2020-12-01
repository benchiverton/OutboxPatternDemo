using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Contract.Commands
{
    public class UpdateState : ICommand
    {
        public string BusinessEntityId { get; set; }
        public StateDetail Details { get; set; }
    }
}
