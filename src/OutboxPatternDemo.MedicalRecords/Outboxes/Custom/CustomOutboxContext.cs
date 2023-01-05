using Microsoft.EntityFrameworkCore;

namespace OutboxPatternDemo.MedicalRecords.Outboxes.Custom;

public class CustomOutboxContext : DbContext
{
    public CustomOutboxContext(DbContextOptions<CustomOutboxContext> options)
        : base(options)
    {
    }

    internal DbSet<CustomOutboxMessage> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.HasDefaultSchema("CustomOutbox");
}
