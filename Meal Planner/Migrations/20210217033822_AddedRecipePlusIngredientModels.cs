using Microsoft.EntityFrameworkCore.Migrations;

namespace Meal_Planner.Migrations
{
    public partial class AddedRecipePlusIngredientModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Meals",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "instructions",
                table: "Meals",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RecipeModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Servings = table.Column<int>(type: "int", nullable: false),
                    ReadyInMinutes = table.Column<int>(type: "int", nullable: false),
                    SourceName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AggregateLikes = table.Column<int>(type: "int", nullable: false),
                    HealthScore = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    PricePerServing = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    DairyFree = table.Column<int>(type: "int", nullable: false),
                    GlutenFree = table.Column<int>(type: "int", nullable: false),
                    Vegan = table.Column<int>(type: "int", nullable: false),
                    Vegetarian = table.Column<int>(type: "int", nullable: false),
                    Ketogenic = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngredientsModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Aisle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Consistency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MetricAmount = table.Column<int>(type: "int", nullable: false),
                    MetricAmountUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImperialAmount = table.Column<int>(type: "int", nullable: false),
                    ImperialUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Original = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecipeModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientsModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientsModel_RecipeModel_RecipeModelId",
                        column: x => x.RecipeModelId,
                        principalTable: "RecipeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientsModel_RecipeModelId",
                table: "IngredientsModel",
                column: "RecipeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeModel_RecipeId",
                table: "RecipeModel",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientsModel");

            migrationBuilder.DropTable(
                name: "RecipeModel");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "instructions",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);
        }
    }
}
