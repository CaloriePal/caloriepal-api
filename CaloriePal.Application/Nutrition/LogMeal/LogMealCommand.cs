using CaloriePal.Application.Nutrition.GetDailyNutrition;
using CaloriePal.Domain.Entities;
using MediatR;

namespace CaloriePal.Application.Nutrition.LogMeal
{
    public sealed record LogMealCommand(Guid UserId, MealCategory Category, Guid? FoodItemId, decimal? QuantityGrams, string? ManualName, decimal? ManualCalories, decimal? ManualProtein, decimal? ManualCarbs, decimal? ManualFat) : IRequest<MealLogDto>;

    public sealed record LogMealRequest(
        Guid? FoodItemId,
        MealCategory Category,
        decimal? QuantityGrams,
        string? ManualName,
        decimal? ManualCalories,
        decimal? ManualProtein,
        decimal? ManualCarbs,
        decimal? ManualFat
    );
}