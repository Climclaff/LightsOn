using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightOn.Migrations
{
    public partial class Amperage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Amperage",
                table: "Appliances",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amperage",
                table: "Appliances");
        }
    }
}
