using Microsoft.EntityFrameworkCore;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices.Data;

public class BusinessEntityContext : DbContext
{
    public BusinessEntityContext(DbContextOptions<BusinessEntityContext> options)
        : base(options)
    {
    }

    public DbSet<StateDetailDto> StateDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.HasDefaultSchema("BusinessEntity");
}
