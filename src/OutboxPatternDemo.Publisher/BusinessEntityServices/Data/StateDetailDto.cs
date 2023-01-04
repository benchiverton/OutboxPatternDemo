using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices.Data;

public class StateDetailDto
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string BusinessEntityId { get; set; }
    public string State { get; set; }
    [Column(TypeName = "datetime2")]
    public DateTime TimeStampUtc { get; set; }
}
