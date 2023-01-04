using Microsoft.EntityFrameworkCore;

namespace OutboxPatternDemo.Subscriber.Data;

public class DuplicateKeyContext : DbContext
{
    public DuplicateKeyContext(DbContextOptions<DuplicateKeyContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<DuplicateKey> DuplicateKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Duplicate");
        modelBuilder.Entity<DuplicateKey>()
            .HasIndex(p => p.Key)
            .IsUnique();
    }
}