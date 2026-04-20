using CaloriePal.Domain.Entities;
using MediatR;

namespace CaloriePal.Application.Workouts.GetWorkoutStats
{
    public sealed record GetWorkoutStatsQuery(Guid UserId) : IRequest<WorkoutStatsDto>;

    public sealed record WorkoutStatsDto(
        int WeeklyGoal,
        int WeeklyCompleted,
        int TotalXp,
        int TimeTrained,
        int SessionCount,
        List<WorkoutSessionDto> RecentSessions
    );

    public sealed record WorkoutSessionDto(
        Guid Id,
        string Name,
        string Category,
        int DurationMinutes,
        int XpAwarded,
        List<WorkoutExerciseDto> Exercises,
        DateTime LoggedAt
    );

    public sealed record WorkoutExerciseDto(
        string ExerciseName,
        int? Sets,
        int? Reps,
        decimal? WeightKg,
        int? DurationMinutes,
        decimal? DistanceKm
    );
}
