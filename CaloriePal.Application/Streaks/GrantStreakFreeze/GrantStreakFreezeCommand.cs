using MediatR;

namespace CaloriePal.Application.Streaks.GrantStreakFreeze
{
    public sealed record GrantStreakFreezeCommand(Guid UserId, int Count = 1) : IRequest<int>;

    public sealed record GrantFreezeRequest(int Count = 1);
}