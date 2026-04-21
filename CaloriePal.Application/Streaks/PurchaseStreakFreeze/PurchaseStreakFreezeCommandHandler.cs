using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Streaks.PurchaseStreakFreeze
{
    public sealed class PurchaseStreakFreezeCommandHandler : IRequestHandler<PurchaseStreakFreezeCommand, PurchaseStreakFreezeResult>
    {
        private readonly IApplicationDbContext _context;
        private const int FreezeCost = 400;

        public PurchaseStreakFreezeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseStreakFreezeResult> Handle(PurchaseStreakFreezeCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            if (profile.Coins < FreezeCost)
                throw new InvalidOperationException("Not enough coins.");

            profile.DeductCoins(FreezeCost);
            profile.AddStreakFreeze();

            await _context.SaveChangesAsync(cancellationToken);

            return new PurchaseStreakFreezeResult(profile.Coins, profile.StreakFreezes);
        }
    }
}