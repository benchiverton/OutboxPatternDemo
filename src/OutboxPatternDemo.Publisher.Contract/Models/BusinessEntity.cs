using System;
using System.Collections.Generic;

namespace OutboxPatternDemo.Publisher.Contract.Models
{
    public class BusinessEntity
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

        public string Id { get; set; }
        public string State { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public List<StateDetail> History { get; set; }
    }
}
