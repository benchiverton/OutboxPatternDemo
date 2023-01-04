using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboxPatternDemo.Publisher.Migrations
{
    /// <inheritdoc />
    public partial class CreateMedicalRecordContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MedicalRecord");

            migrationBuilder.CreateTable(
                name: "AppointmentNotes",
                schema: "MedicalRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresFollowUpAppointment = table.Column<bool>(type: "bit", nullable: false),
                    AppointmentTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentNotes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentNotes",
                schema: "MedicalRecord");
        }
    }
}
