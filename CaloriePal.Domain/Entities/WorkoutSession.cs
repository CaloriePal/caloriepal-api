namespace CaloriePal.Domain.Entities
{
    public class WorkoutSession
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid PlayerId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public WorkoutCategory Category { get; private set; }
        public int DurationMinutes { get; private set; }
        public int XpAwarded { get; private set; }
        public DateOnly LoggedOnDate { get; private set; }
        public DateTime LoggedAt { get; private set; }

        private readonly List<WorkoutExerciseLog> _exercises = new();
        public IReadOnlyList<WorkoutExerciseLog> Exercises => _exercises.AsReadOnly();

        protected WorkoutSession()
        { }

        public static WorkoutSession Create(
            Guid playerId,
            string name,
            WorkoutCategory category,
            int durationMinutes,
            int xpAwarded)
        {
            return new WorkoutSession
            {
                PlayerId = playerId,
                Name = name,
                Category = category,
                DurationMinutes = durationMinutes,
                XpAwarded = xpAwarded,
                LoggedOnDate = DateOnly.FromDateTime(DateTime.UtcNow),
                LoggedAt = DateTime.UtcNow,
            };
        }

        public void AddExercise(WorkoutExerciseLog exercise) => _exercises.Add(exercise);
    }

    public enum WorkoutCategory
    {
        Strength,
        Cardio
    }
}