using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightOn.Migrations
{
    public partial class OneUserPerBuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers",
                column: "BuildingId",
                unique: true,
                filter: "[BuildingId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BuildingId",
                table: "AspNetUsers",
                column: "BuildingId");
        }
    }
}
