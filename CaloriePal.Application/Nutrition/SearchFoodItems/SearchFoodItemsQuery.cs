using MediatR;

namespace CaloriePal.Application.Nutrition.SearchFoodItems
{
    public sealed record SearchFoodItemsQuery(string Term, Guid UserId) : IRequest<List<FoodItemDto>>;

    public sealed record FoodItemDto(
        Guid Id,
        string Name,
        decimal CaloriesPer100g,
        decimal ProteinPer100g,
        decimal CarbsPer100g,
        decimal FatPer100g
    );
}