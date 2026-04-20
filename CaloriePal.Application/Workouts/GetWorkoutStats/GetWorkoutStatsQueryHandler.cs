using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Workouts.GetWorkoutStats
{
    public sealed class GetWorkoutStatsQueryHandler : IRequestHandler<GetWorkoutStatsQuery, WorkoutStatsDto>
    {
        private const int WeeklyGoal = 5;
        private const int RecentSessionsLimit = 20;

        private readonly IApplicationDbContext _context;

        public GetWorkoutStatsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutStatsDto> Handle(GetWorkoutStatsQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var weekStart = today.AddDays(-(int)today.DayOfWeek == 0 ? 6 : (int)today.DayOfWeek - 1);

            var allSessions = await _context.WorkoutSessions
                .Where(s => s.PlayerId == profile.Id)
                .Include(s => s.Exercises)
                .OrderByDescending(s => s.LoggedAt)
                .ToListAsync(cancellationToken);

            var weeklyCompleted = allSessions.Count(s => s.LoggedOnDate >= weekStart);
            var totalXp = allSessions.Sum(s => s.XpAwarded);
            var timeTrained = allSessions.Sum(s => s.DurationMinutes);
            var sessionCount = allSessions.Count;

            var recentDtos = allSessions
                .Take(RecentSessionsLimit)
                .Select(s => new WorkoutSessionDto(
                    s.Id,
                    s.Name,
                    s.Category.ToString(),
                    s.DurationMinutes,
                    s.XpAwarded,
                    s.Exercises.Select(e => new WorkoutExerciseDto(
                        e.ExerciseName,
                        e.Sets,
                        e.Reps,
                        e.WeightKg,
                        e.DurationMinutes,
                        e.DistanceKm
                    )).ToList(),
                    s.LoggedAt
                ))
                .ToList();

            return new WorkoutStatsDto(
                WeeklyGoal,
                weeklyCompleted,
                totalXp,
                timeTrained,
                sessionCount,
                recentDtos
            );
        }
    }
}
