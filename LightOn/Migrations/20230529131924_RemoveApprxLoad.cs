using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightOn.Migrations
{
    public partial class RemoveApprxLoad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproximateLoad",
                table: "ApplianceUsagePlanneds");

            migrationBuilder.DropColumn(
                name: "ApproximateLoad",
                table: "ApplianceUsageHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ApproximateLoad",
                table: "ApplianceUsagePlanneds",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ApproximateLoad",
                table: "ApplianceUsageHistories",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
