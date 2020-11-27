using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxPatternDemo.Publisher.Infrastructure
{
    internal class OutboxMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string Type { get; internal set; }
        public string Data { get; internal set; }
        public DateTime RequestedTimeUtc { get; internal set; }
        public DateTime? ProcessedTimeUtc { get; internal set; }

        /// <summary>
        /// If SendDuplicate is set to true, then a this message will be sent twice.
        /// This is to mimic a scenario where the DB update fails.
        /// </summary>
        public bool SendDuplicate { get; internal set; }
    }
}
