using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightOn.Migrations
{
    public partial class BuildingArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Area",
                table: "Buildings",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Buildings");
        }
    }
}
