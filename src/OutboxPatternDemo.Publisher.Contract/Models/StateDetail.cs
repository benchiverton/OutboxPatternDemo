using System;

namespace OutboxPatternDemo.Publisher.Contract.Models;

public record StateDetail(int Id, string State, DateTime TimeStampUtc);
