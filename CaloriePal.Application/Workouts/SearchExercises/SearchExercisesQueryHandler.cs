using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Workouts.SearchExercises
{
    public sealed class SearchExercisesQueryHandler : IRequestHandler<SearchExercisesQuery, List<ExerciseDto>>
    {
        private readonly IApplicationDbContext _context;

        public SearchExercisesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExerciseDto>> Handle(SearchExercisesQuery request, CancellationToken cancellationToken)
        {
            var term = request.Term.ToLower().Trim();

            return await _context.Exercises
                .Where(e => e.Name.ToLower().Contains(term))
                .OrderBy(e => e.Name)
                .Take(20)
                .Select(e => new ExerciseDto(
                    e.Id,
                    e.Name,
                    e.Category.ToString(),
                    e.MuscleGroup
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
