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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Quest>()
                .Property(q => q.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Quest>()
                .Property(q => q.Type)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}