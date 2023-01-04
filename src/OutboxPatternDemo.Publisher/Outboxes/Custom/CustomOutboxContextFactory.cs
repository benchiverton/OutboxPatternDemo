using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OutboxPatternDemo.Publisher.Outboxes.Custom;

public class CustomOutboxContextFactory : IDesignTimeDbContextFactory<CustomOutboxContext>
{
    public CustomOutboxContext CreateDbContext(string[] args)
    {
        var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True");
        var contextOptionsBuilder = new DbContextOptionsBuilder<CustomOutboxContext>();
        contextOptionsBuilder.UseSqlServer(sqlConnection);
        return new CustomOutboxContext(contextOptionsBuilder.Options);
    }
}
