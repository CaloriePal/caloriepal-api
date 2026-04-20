namespace CaloriePal.Domain.Entities
{
    public class WorkoutExerciseLog
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid WorkoutSessionId { get; private set; }
        public Guid? ExerciseId { get; private set; }
        public string ExerciseName { get; private set; } = string.Empty;
        public int? Sets { get; private set; }
        public int? Reps { get; private set; }
        public decimal? WeightKg { get; private set; }
        public int? DurationMinutes { get; private set; }
        public decimal? DistanceKm { get; private set; }

        protected WorkoutExerciseLog() { }

        public static WorkoutExerciseLog Create(
            Guid workoutSessionId,
            string exerciseName,
            Guid? exerciseId = null,
            int? sets = null,
            int? reps = null,
            decimal? weightKg = null,
            int? durationMinutes = null,
            decimal? distanceKm = null)
        {
            return new WorkoutExerciseLog
            {
                WorkoutSessionId = workoutSessionId,
                ExerciseId = exerciseId,
                ExerciseName = exerciseName,
                Sets = sets,
                Reps = reps,
                WeightKg = weightKg,
                DurationMinutes = durationMinutes,
                DistanceKm = distanceKm,
            };
        }
    }
}
