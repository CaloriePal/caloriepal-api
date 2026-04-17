using MediatR;

namespace CaloriePal.Application.Quests.GetDailyQuests
{
    public sealed record GetDailyQuestsQuery(Guid UserId) : IRequest<DailyQuestsDto>;

    public sealed record DailyQuestsDto(
        List<DailyQuestItemDto> Quests,
        int CompletedCount,
        int TotalXpEarned,
        int TotalCoinsEarned
    );

    public sealed record DailyQuestItemDto(
        Guid QuestId,
        string Title,
        string Description,
        string Category,
        int XpReward,
        int CoinReward,
        bool IsCompleted
    );
}