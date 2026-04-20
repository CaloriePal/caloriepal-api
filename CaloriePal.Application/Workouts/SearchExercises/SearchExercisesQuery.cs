using MediatR;

namespace CaloriePal.Application.Workouts.SearchExercises
{
    public sealed record SearchExercisesQuery(string Term) : IRequest<List<ExerciseDto>>;

    public sealed record ExerciseDto(
        Guid Id,
        string Name,
        string Category,
        string? MuscleGroup
    );
}
