using Microsoft.EntityFrameworkCore.Migrations;

namespace PjoterParker.Api.Database.Migrations
{
    public partial class AddUniquenessTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UniquenessTable",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 1024, nullable: false),
                    Value = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniquenessTable", x => new { x.Key, x.Value });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniquenessTable");
        }
    }
}
