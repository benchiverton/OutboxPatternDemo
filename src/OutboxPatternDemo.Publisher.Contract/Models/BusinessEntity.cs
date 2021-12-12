using System;
using System.Collections.Generic;

namespace OutboxPatternDemo.Publisher.Contract.Models;

public record BusinessEntity
{
    public BusinessEntity(string id, List<StateDetail> stateDetails)
    {
        Id = id;
        History = new List<StateDetail>();
        foreach (var stateDetail in stateDetails)
        {
            History.Add(stateDetail);
            if (LastUpdatedUtc < stateDetail.TimeStampUtc)
            {
                State = stateDetail.State;
                LastUpdatedUtc = stateDetail.TimeStampUtc;
            }
        }
    }

    public string Id { get; }
    public string State { get; }
    public DateTime LastUpdatedUtc { get; }
    public List<StateDetail> History { get; }
}
