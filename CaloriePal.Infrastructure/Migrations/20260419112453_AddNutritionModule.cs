using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaloriePal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNutritionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CaloriesPer100g = table.Column<decimal>(type: "numeric", nullable: false),
                    ProteinPer100g = table.Column<decimal>(type: "numeric", nullable: false),
                    CarbsPer100g = table.Column<decimal>(type: "numeric", nullable: false),
                    FatPer100g = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoodItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    FoodName = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Calories = table.Column<decimal>(type: "numeric", nullable: false),
                    Protein = table.Column<decimal>(type: "numeric", nullable: false),
                    Carbs = table.Column<decimal>(type: "numeric", nullable: false),
                    Fat = table.Column<decimal>(type: "numeric", nullable: false),
                    LoggedOnDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LoggedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "MealLogs");
        }
    }
}