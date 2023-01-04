using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OutboxPatternDemo.Publisher.MedicalRecords.Data;

public class MedicalRecordContextFactory : IDesignTimeDbContextFactory<MedicalRecordContext>
{
    public MedicalRecordContext CreateDbContext(string[] args)
    {
        var sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=OutboxPatternDemo;Integrated Security=SSPI;TrustServerCertificate=True");
        var contextOptionsBuilder = new DbContextOptionsBuilder<MedicalRecordContext>();
        contextOptionsBuilder.UseSqlServer(sqlConnection);
        return new MedicalRecordContext(contextOptionsBuilder.Options);
    }
}
