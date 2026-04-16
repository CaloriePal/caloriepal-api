using CaloriePal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PlayerProfile> PlayerProfiles { get; }
        DbSet<XpEvent> XpEvents { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}