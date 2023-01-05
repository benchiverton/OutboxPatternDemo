using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboxPatternDemo.Publisher.Migrations.CustomOutbox
{
    /// <inheritdoc />
    public partial class CreateCustomOutboxContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CustomOutbox");

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "CustomOutbox",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "CustomOutbox");
        }
    }
}
