using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OutboxPatternDemo.Subscriber.Data
{
    public class DuplicateKeyContextDesignTimeFactory : IDesignTimeDbContextFactory<DuplicateKeyContext>
    {
        public DuplicateKeyContext CreateDbContext(string[] args)
        {
            var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI");
            var contextOptionsBuilder = new DbContextOptionsBuilder<DuplicateKeyContext>();
            contextOptionsBuilder.UseSqlServer(sqlConnection);
            return new DuplicateKeyContext(contextOptionsBuilder.Options);
        }
    }
}
