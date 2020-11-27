using Microsoft.EntityFrameworkCore;

namespace OutboxPatternDemo.Publisher.Infrastructure
{
    public class OutboxContext : DbContext
    {
        public OutboxContext(DbContextOptions<OutboxContext> options)
            : base(options)
        {
        }

        internal DbSet<OutboxMessage> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.HasDefaultSchema("Outbox");
    }
}
