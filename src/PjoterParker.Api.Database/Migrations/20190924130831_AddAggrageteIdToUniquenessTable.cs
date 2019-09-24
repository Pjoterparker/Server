using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PjoterParker.Api.Database.Migrations
{
    public partial class AddAggrageteIdToUniquenessTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UniquenessTable",
                table: "UniquenessTable");

            migrationBuilder.AddColumn<Guid>(
                name: "AggrageteId",
                table: "UniquenessTable",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniquenessTable",
                table: "UniquenessTable",
                columns: new[] { "Key", "Value", "AggrageteId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UniquenessTable",
                table: "UniquenessTable");

            migrationBuilder.DropColumn(
                name: "AggrageteId",
                table: "UniquenessTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniquenessTable",
                table: "UniquenessTable",
                columns: new[] { "Key", "Value" });
        }
    }
}
