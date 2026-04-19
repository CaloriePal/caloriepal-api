using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Nutrition.GetDailyNutrition;
using CaloriePal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Nutrition.LogMeal
{
    public sealed class LogMealCommandHandler : IRequestHandler<LogMealCommand, MealLogDto>
    {
        private readonly IApplicationDbContext _context;

        public LogMealCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MealLogDto> Handle(LogMealCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile for User ID {request.UserId} not found.");

            MealLog entry;

            if (request.FoodItemId.HasValue)
            {
                var food = await _context.FoodItems
                    .FirstOrDefaultAsync(f => f.Id == request.FoodItemId.Value, cancellationToken)
                    ?? throw new InvalidOperationException($"FoodItem with ID {request.FoodItemId} not found.");

                entry = MealLog.FromFood(
                    profile.Id, food,
                    request.Category, request.QuantityGrams!.Value);
            }
            else
            {
                entry = MealLog.FromManual(
                    profile.Id,
                    request.ManualName!,
                    request.Category,
                    request.ManualCalories!.Value,
                    request.ManualProtein!.Value,
                    request.ManualCarbs!.Value,
                    request.ManualFat!.Value);
            }

            _context.MealLogs.Add(entry);
            await _context.SaveChangesAsync(cancellationToken);

            return new MealLogDto
            (
                Id: entry.Id,
                FoodName: entry.FoodName,
                Category: entry.Category.ToString(),
                Calories: entry.Calories,
                Protein: entry.Protein,
                Carbs: entry.Carbs,
                Fat: entry.Fat,
                LoggedAt: entry.LoggedAt
            );
        }
    }
}