using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices.Data;

public class BusinessEntityContextFactory : IDesignTimeDbContextFactory<BusinessEntityContext>
{
    public BusinessEntityContext CreateDbContext(string[] args)
    {
        var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI");
        var contextOptionsBuilder = new DbContextOptionsBuilder<BusinessEntityContext>();
        contextOptionsBuilder.UseSqlServer(sqlConnection);
        return new BusinessEntityContext(contextOptionsBuilder.Options);
    }
}
