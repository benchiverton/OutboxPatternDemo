using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OutboxPatternDemo.Publisher.Migrations.BusinessEntity
{
    public partial class CreateBusinessEntityTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BusinessEntity");

            migrationBuilder.CreateTable(
                name: "StateDetails",
                schema: "BusinessEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BusinessEntityId = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    TimeStampUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateDetails",
                schema: "BusinessEntity");
        }
    }
}
