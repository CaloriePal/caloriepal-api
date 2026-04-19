using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Nutrition.SearchFoodItems
{
    public sealed class SearchFoodItemsQueryHandler : IRequestHandler<SearchFoodItemsQuery, List<FoodItemDto>>
    {
        private readonly IApplicationDbContext _context;

        public SearchFoodItemsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodItemDto>> Handle(SearchFoodItemsQuery request, CancellationToken cancellationToken)
        {
            var term = request.Term.ToLower().Trim();

            return await _context.FoodItems
                .Where(f => f.Name.ToLower().Contains(term))
                .OrderBy(f => f.Name)
                .Take(20)
                .Select(f => new FoodItemDto(
                    f.Id,
                    f.Name,
                    f.CaloriesPer100g,
                    f.ProteinPer100g,
                    f.CarbsPer100g,
                    f.FatPer100g
                ))
                .ToListAsync(cancellationToken);
        }
    }
}