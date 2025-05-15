using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryManagerLib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepositoryItems",
                columns: table => new
                {
                    ItemName = table.Column<string>(type: "text", nullable: false),
                    ItemContent = table.Column<string>(type: "text", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryItems", x => x.ItemName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryItems");
        }
    }
}
