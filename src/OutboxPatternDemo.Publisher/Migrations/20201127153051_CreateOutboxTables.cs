using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OutboxPatternDemo.Publisher.Migrations
{
    public partial class CreateOutboxTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Outbox");

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Outbox",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    RequestedTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SendDuplicate = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Outbox");
        }
    }
}
