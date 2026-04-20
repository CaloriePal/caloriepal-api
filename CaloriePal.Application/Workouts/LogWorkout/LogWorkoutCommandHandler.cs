using CaloriePal.Application.Interfaces;
using CaloriePal.Application.Workouts.GetWorkoutStats;
using CaloriePal.Domain.Entities;
using CaloriePal.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Workouts.LogWorkout
{
    public sealed class LogWorkoutCommandHandler : IRequestHandler<LogWorkoutCommand, WorkoutSessionDto>
    {
        private static readonly Dictionary<WorkoutCategory, double> XpMultipliers = new()
        {
            { WorkoutCategory.Strength,    1.2 },
            { WorkoutCategory.Cardio,      1.0 },
        };

        private readonly IApplicationDbContext _context;
        private readonly ILevelingService _levelingService;

        public LogWorkoutCommandHandler(IApplicationDbContext context, ILevelingService levelingService)
        {
            _context = context;
            _levelingService = levelingService;
        }

        public async Task<WorkoutSessionDto> Handle(LogWorkoutCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            var multiplier = XpMultipliers.GetValueOrDefault(request.Category, 1.0);
            var rawXp = request.DurationMinutes * 2 * multiplier;
            var xpAwarded = Math.Max(10, (int)(Math.Round(rawXp / 5.0) * 5));

            var session = WorkoutSession.Create(
                profile.Id,
                request.Name,
                request.Category,
                request.DurationMinutes,
                xpAwarded);

            foreach (var e in request.Exercises)
            {
                var log = WorkoutExerciseLog.Create(
                    session.Id,
                    e.ExerciseName,
                    e.ExerciseId,
                    e.Sets,
                    e.Reps,
                    e.WeightKg,
                    e.DurationMinutes,
                    e.DistanceKm);
                session.AddExercise(log);
            }

            _context.WorkoutSessions.Add(session);

            profile.AddXp(xpAwarded, _levelingService);
            var xpEvent = XpEvent.Create(profile.Id, xpAwarded, "workout");
            _context.XpEvents.Add(xpEvent);

            await _context.SaveChangesAsync(cancellationToken);

            return new WorkoutSessionDto(
                session.Id,
                session.Name,
                session.Category.ToString(),
                session.DurationMinutes,
                session.XpAwarded,
                session.Exercises.Select(e => new WorkoutExerciseDto(
                    e.ExerciseName,
                    e.Sets,
                    e.Reps,
                    e.WeightKg,
                    e.DurationMinutes,
                    e.DistanceKm
                )).ToList(),
                session.LoggedAt
            );
        }
    }
}