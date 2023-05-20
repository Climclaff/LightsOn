using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightOn.Migrations
{
    public partial class historySeparation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplianceUsagePlanneds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsageStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproximateLoad = table.Column<float>(type: "real", nullable: false),
                    ApplianceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplianceUsagePlanneds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplianceUsagePlanneds_Appliances_ApplianceId",
                        column: x => x.ApplianceId,
                        principalTable: "Appliances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplianceUsagePlanneds_ApplianceId",
                table: "ApplianceUsagePlanneds",
                column: "ApplianceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplianceUsagePlanneds");
        }
    }
}
