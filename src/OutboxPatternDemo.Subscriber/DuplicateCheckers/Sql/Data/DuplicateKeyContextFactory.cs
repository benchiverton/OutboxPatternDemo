using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers.Sql.Data;

public class DuplicateKeyContextFactory : IDesignTimeDbContextFactory<DuplicateKeyContext>
{
    public DuplicateKeyContext CreateDbContext(string[] args)
    {
        var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True");
        var contextOptionsBuilder = new DbContextOptionsBuilder<DuplicateKeyContext>();
        contextOptionsBuilder.UseSqlServer(sqlConnection);
        return new DuplicateKeyContext(contextOptionsBuilder.Options);
    }
}
