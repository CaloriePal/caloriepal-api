using CaloriePal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PlayerProfile> PlayerProfiles { get; }
        DbSet<XpEvent> XpEvents { get; }
        DbSet<Quest> Quests { get; }
        DbSet<PlayerQuestLog> PlayerQuestLogs { get; }
        DbSet<FoodItem> FoodItems { get; }
        DbSet<MealLog> MealLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}