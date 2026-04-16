using CaloriePal.Application.Interfaces;
using CaloriePal.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Streaks.UpdateStreak
{
    public sealed class UpdateStreakCommandHandler : IRequestHandler<UpdateStreakCommand, UpdateStreakResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateStreakCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateStreakResult> Handle(UpdateStreakCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var result = profile.UpdateStreak(today);

            await _context.SaveChangesAsync(cancellationToken);

            string outcome = result.Type switch
            {
                StreakUpdateResultType.AlreadyLoggedToday => "already_logged",
                StreakUpdateResultType.StreakStarted => "started",
                StreakUpdateResultType.StreakExtended => "extended",
                StreakUpdateResultType.FreezeConsumed => "freeze_consumed",
                StreakUpdateResultType.StreakBroken => "broken",
                _ => "unknown"
            };

            return new UpdateStreakResult(
                CurrentStreak: profile.CurrentStreak,
                LongestStreak: profile.LongestStreak,
                Outcome: outcome,
                LostStreakLength: result.LostStreakLength
            );
        }
    }
}