using System.ComponentModel.DataAnnotations.Schema;

namespace OutboxPatternDemo.Bookings.DuplicateCheckers.Sql.Data;

public class DuplicateKey
{
    public DuplicateKey(string key) => Key = key;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Key { get; }
}