using CaloriePal.Application.Interfaces;
using CaloriePal.Domain;
using CaloriePal.Domain.Entities;
using CaloriePal.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Quests.CompleteQuest
{
    public sealed class CompleteQuestCommandHandler : IRequestHandler<CompleteQuestCommand, CompleteQuestResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILevelingService _levelingService;

        public CompleteQuestCommandHandler(IApplicationDbContext context, ILevelingService levelingService)
        {
            _context = context;
            _levelingService = levelingService;
        }

        public async Task<CompleteQuestResult> Handle(CompleteQuestCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException("PlayerProfile not found.");

            var quest = await _context.Quests
                .FirstOrDefaultAsync(q => q.Id == request.QuestId && q.IsActive, cancellationToken)
                ?? throw new InvalidOperationException("Quest not found or inactive.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            bool alreadyDone = await _context.PlayerQuestLogs
                .AnyAsync(l =>
                    l.PlayerProfileId == profile.Id &&
                    l.QuestId == quest.Id &&
                    l.CompletedOnDate == today,
                    cancellationToken);

            if (alreadyDone)
                throw new InvalidOperationException("Quest already completed today.");

            var now = DateTime.UtcNow;

            // 1. Award XP
            int levelsGained = profile.AddXp(quest.XpReward, _levelingService);
            var xpEvent = XpEvent.Create(profile.Id, quest.XpReward, "quest");
            _context.XpEvents.Add(xpEvent);

            // 2. Award coins
            profile.AddCoins(quest.CoinReward);

            // 3. Update streak
            var streakResult = profile.UpdateStreak(today);

            // 4. Log quest completion
            var log = PlayerQuestLog.Create(profile.Id, quest.Id, now);
            _context.PlayerQuestLogs.Add(log);

            await _context.SaveChangesAsync(cancellationToken);

            string streakOutcome = streakResult.Type switch
            {
                StreakUpdateResultType.AlreadyLoggedToday => "already_logged",
                StreakUpdateResultType.StreakStarted => "started",
                StreakUpdateResultType.StreakExtended => "extended",
                StreakUpdateResultType.FreezeConsumed => "freeze_consumed",
                StreakUpdateResultType.StreakBroken => "broken",
                _ => "unknown"
            };

            return new CompleteQuestResult(
                XpAwarded: quest.XpReward,
                CoinsAwarded: quest.CoinReward,
                NewTotalXp: profile.TotalXp,
                NewLevel: profile.Level,
                LevelsGained: levelsGained,
                NewStreak: profile.CurrentStreak,
                StreakOutcome: streakOutcome,
                TotalCoins: profile.Coins
            );
        }
    }
}