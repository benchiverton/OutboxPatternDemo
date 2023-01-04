using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboxPatternDemo.Publisher.Migrations.BusinessEntity
{
    /// <inheritdoc />
    public partial class BusinessEntityContextCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BusinessEntity");

            migrationBuilder.CreateTable(
                name: "StateDetails",
                schema: "BusinessEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessEntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStampUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateDetails",
                schema: "BusinessEntity");
        }
    }
}
