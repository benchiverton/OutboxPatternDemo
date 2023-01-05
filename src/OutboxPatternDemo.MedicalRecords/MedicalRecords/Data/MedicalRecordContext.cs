using Microsoft.EntityFrameworkCore;

namespace OutboxPatternDemo.MedicalRecords.MedicalRecords.Data;

public class MedicalRecordContext : DbContext
{
    public MedicalRecordContext(DbContextOptions<MedicalRecordContext> options)
        : base(options)
    {
    }

    public DbSet<AppointmentNotesDto> AppointmentNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.HasDefaultSchema("MedicalRecord");
}
