using Microsoft.EntityFrameworkCore.Migrations;

namespace Meal_Planner.Migrations
{
    public partial class addedGlutenIntolerance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GlutenIntolerance",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlutenIntolerance",
                table: "AspNetUsers");
        }
    }
}
