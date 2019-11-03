using Microsoft.EntityFrameworkCore.Migrations;

namespace PjoterParker.Api.Database.Migrations
{
    public partial class Add_Column_IsDisabled_Spot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Spot",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Spot");
        }
    }
}
