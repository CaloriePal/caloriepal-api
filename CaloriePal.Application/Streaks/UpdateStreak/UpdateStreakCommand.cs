using MediatR;

namespace CaloriePal.Application.Streaks.UpdateStreak
{
    public sealed record UpdateStreakCommand(string UserId) : IRequest<UpdateStreakResult>;

    public sealed record UpdateStreakResult(
        int CurrentStreak,
        int LongestStreak,
        string Outcome,
        int? LostStreakLength
    );
}