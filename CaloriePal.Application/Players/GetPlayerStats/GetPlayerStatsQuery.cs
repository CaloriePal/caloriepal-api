using MediatR;

namespace CaloriePal.Application.Players.GetPlayerStats
{
    public sealed record GetPlayerStatsQuery(Guid UserId) : IRequest<PlayerStatsDto>;

    public sealed record PlayerStatsDto(
        Guid ProfileId,
        string DisplayName,
        string? AvatarUrl,
        int Level,
        string Title,
        int TotalXp,
        int XpIntoCurrentLevel,
        int XpRequiredForNextLevel,
        int Coins,
        int CurrentStreak,
        int LongestStreak,
        int StreakFreezes,
        int QuestsDoneThisMonth
    );
}