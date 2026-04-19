using MediatR;

namespace CaloriePal.Application.Nutrition.GetDailyNutrition
{
    public sealed record GetDailyNutritionQuery(Guid UserId, DateOnly Date) : IRequest<DailyNutritionDto>;

    public sealed record DailyNutritionDto(
        DateOnly Date,
        decimal Calories,
        decimal Protein,
        decimal Carbs,
        decimal Fat,
        List<MealLogDto> Meals
    );

    public sealed record MealLogDto(
        Guid Id,
        string FoodName,
        string Category,
        decimal Calories,
        decimal Protein,
        decimal Carbs,
        decimal Fat,
        DateTime LoggedAt
    );
}