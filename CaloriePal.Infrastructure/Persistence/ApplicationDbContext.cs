using CaloriePal.Application.Interfaces;
using CaloriePal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
    {
        public DbSet<PlayerProfile> PlayerProfiles => Set<PlayerProfile>();
        public DbSet<XpEvent> XpEvents => Set<XpEvent>();
        public DbSet<Quest> Quests => Set<Quest>();
        public DbSet<PlayerQuestLog> PlayerQuestLogs => Set<PlayerQuestLog>();
        public DbSet<FoodItem> FoodItems => Set<FoodItem>();
        public DbSet<MealLog> MealLogs => Set<MealLog>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
        public DbSet<WorkoutExerciseLog> WorkoutExerciseLogs => Set<WorkoutExerciseLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Quest>()
                .Property(q => q.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Quest>()
                .Property(q => q.Type)
                .HasConversion<string>();

            modelBuilder.Entity<WorkoutSession>()
                .Property(s => s.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Exercise>()
                .Property(e => e.Category)
                .HasConversion<string>();

            modelBuilder.Entity<WorkoutSession>()
                .HasMany(s => s.Exercises)
                .WithOne()
                .HasForeignKey(e => e.WorkoutSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}