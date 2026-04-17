using CaloriePal.Application.Interfaces;
using CaloriePal.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Players.GetPlayerStats
{
    public sealed class GetPlayerStatsQueryHandler : IRequestHandler<GetPlayerStatsQuery, PlayerStatsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILevelingService _levelingService;
        private readonly ITitleService _titleService;

        public GetPlayerStatsQueryHandler(
            IApplicationDbContext context,
            ILevelingService levelingService,
            ITitleService titleService)
        {
            _context = context;
            _levelingService = levelingService;
            _titleService = titleService;
        }

        public async Task<PlayerStatsDto> Handle(GetPlayerStatsQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            int questsDoneThisMonth = await _context.PlayerQuestLogs
                .AsNoTracking()
                .CountAsync(q => q.PlayerProfileId == profile.Id && q.CompletedAt >= startOfMonth, cancellationToken);

            return new PlayerStatsDto(
                ProfileId: profile.Id,
                DisplayName: profile.DisplayName,
                AvatarUrl: profile.AvatarUrl,
                Level: profile.Level,
                Title: _titleService.GetTitle(profile.Level),
                TotalXp: profile.TotalXp,
                XpIntoCurrentLevel: profile.XpIntoCurrentLevel,
                XpRequiredForNextLevel: _levelingService.XpRequiredForNextLevel(profile.Level),
                Coins: profile.Coins,
                CurrentStreak: profile.CurrentStreak,
                LongestStreak: profile.LongestStreak,
                StreakFreezes: profile.StreakFreezes,
                QuestsDoneThisMonth: questsDoneThisMonth
            );
        }
    }
}