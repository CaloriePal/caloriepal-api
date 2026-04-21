using MediatR;

namespace CaloriePal.Application.Streaks.PurchaseStreakFreeze
{
    public sealed record PurchaseStreakFreezeCommand(Guid UserId) : IRequest<PurchaseStreakFreezeResult>;

    public sealed record PurchaseStreakFreezeResult(int NewCoinBalance, int NewStreakFreezeCount);
}