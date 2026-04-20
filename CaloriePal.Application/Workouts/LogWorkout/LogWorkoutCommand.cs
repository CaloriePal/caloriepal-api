using CaloriePal.Application.Workouts.GetWorkoutStats;
using CaloriePal.Domain.Entities;
using MediatR;

namespace CaloriePal.Application.Workouts.LogWorkout
{
    public sealed record LogWorkoutCommand(
        Guid UserId,
        string Name,
        WorkoutCategory Category,
        int DurationMinutes,
        List<WorkoutExerciseEntry> Exercises
    ) : IRequest<WorkoutSessionDto>;

    public sealed record WorkoutExerciseEntry(
        string ExerciseName,
        Guid? ExerciseId,
        int? Sets,
        int? Reps,
        decimal? WeightKg,
        int? DurationMinutes,
        decimal? DistanceKm
    );

    public sealed record LogWorkoutRequest(
        string Name,
        WorkoutCategory Category,
        int DurationMinutes,
        List<WorkoutExerciseEntry> Exercises
    );
}
