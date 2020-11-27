using System;

namespace OutboxPatternDemo.Publisher.Contract.Models
{
    public class StateDetail
    {
        public int Id { get; set; }
        public string State { get; set; }
        public DateTime TimeStampUtc { get; set; }
    }
}
