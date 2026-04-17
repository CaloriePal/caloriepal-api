using CaloriePal.Application.Interfaces;
using CaloriePal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Quests.GetDailyQuests
{
    public sealed class GetDailyQuestsQueryHandler : IRequestHandler<GetDailyQuestsQuery, DailyQuestsDto>
    {
        private readonly IApplicationDbContext _context;

        public GetDailyQuestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DailyQuestsDto> Handle(GetDailyQuestsQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var quests = await _context.Quests
                .AsNoTracking()
                .Where(q => q.IsActive && q.Type == QuestType.Daily)
                .OrderBy(q => q.SortOrder)
                .ToListAsync(cancellationToken);

            var completedTodayIds = await _context.PlayerQuestLogs
                .AsNoTracking()
                .Where(l => l.PlayerProfileId == profile.Id && l.CompletedOnDate == today)
                .Select(l => l.QuestId)
                .ToHashSetAsync(cancellationToken);

            var items = quests.Select(q => new DailyQuestItemDto(
                QuestId: q.Id,
                Title: q.Title,
                Description: q.Description,
                Category: q.Category.ToString(),
                XpReward: q.XpReward,
                CoinReward: q.CoinReward,
                IsCompleted: completedTodayIds.Contains(q.Id)
            )).ToList();

            int completedCount = items.Count(i => i.IsCompleted);
            int totalXpEarned = items.Where(i => i.IsCompleted).Sum(i => i.XpReward);
            int totalCoinsEarned = items.Where(i => i.IsCompleted).Sum(i => i.CoinReward);

            return new DailyQuestsDto(items, completedCount, totalXpEarned, totalCoinsEarned);
        }
    }
}