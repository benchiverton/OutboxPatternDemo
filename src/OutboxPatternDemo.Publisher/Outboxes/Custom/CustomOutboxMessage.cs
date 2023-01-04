using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxPatternDemo.Publisher.Outboxes.Custom;

internal class CustomOutboxMessage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Type { get; internal set; }
    public string Data { get; internal set; }
    public DateTime RequestedTimeUtc { get; internal set; }
    public DateTime? ProcessedTimeUtc { get; internal set; }
}