using NServiceBus;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.Contract.Commands;

public record UpdateState(string BusinessEntityId, StateDetail Details) : ICommand;
