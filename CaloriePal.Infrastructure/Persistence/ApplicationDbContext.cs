using CaloriePal.Application.Interfaces;
using CaloriePal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
    {
        public DbSet<PlayerProfile> PlayerProfiles => Set<PlayerProfile>();
        public DbSet<XpEvent> XpEvents => Set<XpEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}