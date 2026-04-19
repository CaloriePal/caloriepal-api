using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Nutrition.GetDailyNutrition
{
    public sealed class GetDailyNutritionQueryHandler : IRequestHandler<GetDailyNutritionQuery, DailyNutritionDto>
    {
        private readonly IApplicationDbContext _context;

        public GetDailyNutritionQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DailyNutritionDto> Handle(GetDailyNutritionQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            var meals = await _context.MealLogs
                .Where(m => m.PlayerId == profile.Id && m.LoggedOnDate == request.Date)
                .OrderBy(m => m.LoggedAt)
                .ToListAsync();

            var mealDtos = meals.Select(m => new MealLogDto(
                m.Id,
                m.FoodName,
                m.Category.ToString(),
                m.Calories,
                m.Protein,
                m.Carbs,
                m.Fat,
                m.LoggedAt
            )).ToList();

            return new DailyNutritionDto(
                request.Date,
                meals.Sum(m => m.Calories),
                meals.Sum(m => m.Protein),
                meals.Sum(m => m.Carbs),
                meals.Sum(m => m.Fat),
                mealDtos
            );
        }
    }
}