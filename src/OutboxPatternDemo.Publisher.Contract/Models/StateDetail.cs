using System;

namespace OutboxPatternDemo.Publisher.Contract.Models;

public record StateDetail(Guid Id, string State, DateTime TimeStampUtc)
{
    public Guid Id { get; init; } = Id == default ? Guid.NewGuid() : Id;
};
