using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices.Data
{
    public class StateDetailDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BusinessEntityId { get; set; }
        public string State { get; set; }
        public DateTime TimeStampUtc { get; set; }
    }
}
