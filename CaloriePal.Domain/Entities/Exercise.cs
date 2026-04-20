namespace CaloriePal.Domain.Entities
{
    public class Exercise
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public WorkoutCategory Category { get; private set; }
        public string? MuscleGroup { get; private set; }

        protected Exercise() { }

        public static Exercise Create(string name, WorkoutCategory category, string? muscleGroup = null)
        {
            return new Exercise
            {
                Name = name,
                Category = category,
                MuscleGroup = muscleGroup,
            };
        }
    }
}
