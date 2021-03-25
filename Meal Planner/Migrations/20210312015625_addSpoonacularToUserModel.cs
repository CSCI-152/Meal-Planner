using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Meal_Planner.Migrations
{
    public partial class addSpoonacularToUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpoonAccountId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MealPlanUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlanUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpoonAccountId",
                table: "AspNetUsers",
                column: "SpoonAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MealPlanUser_SpoonAccountId",
                table: "AspNetUsers",
                column: "SpoonAccountId",
                principalTable: "MealPlanUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MealPlanUser_SpoonAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MealPlanUser");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SpoonAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpoonAccountId",
                table: "AspNetUsers");
        }
    }
}
