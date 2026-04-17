using MediatR;

namespace CaloriePal.Application.Quests.CompleteQuest
{
    public sealed record CompleteQuestCommand(Guid UserId, Guid QuestId) : IRequest<CompleteQuestResult>;

    public sealed record CompleteQuestResult(
        int XpAwarded,
        int CoinsAwarded,
        int NewTotalXp,
        int NewLevel,
        int LevelsGained,
        int NewStreak,
        string StreakOutcome,
        int TotalCoins
    );
}