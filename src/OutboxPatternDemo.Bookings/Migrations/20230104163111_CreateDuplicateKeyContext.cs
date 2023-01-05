using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboxPatternDemo.Subscriber.Migrations
{
    /// <inheritdoc />
    public partial class CreateDuplicateKeyContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Duplicate");

            migrationBuilder.CreateTable(
                name: "DuplicateKeys",
                schema: "Duplicate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuplicateKeys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuplicateKeys_Key",
                schema: "Duplicate",
                table: "DuplicateKeys",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuplicateKeys",
                schema: "Duplicate");
        }
    }
}
