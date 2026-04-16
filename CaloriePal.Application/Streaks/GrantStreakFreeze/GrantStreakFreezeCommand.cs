using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Streaks.GrantStreakFreeze
{
    public sealed record GrantStreakFreezeCommand(string UserId, int Count = 1) : IRequest<int>;

    public sealed class GrantStreakFreezeCommandHandler : IRequestHandler<GrantStreakFreezeCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public GrantStreakFreezeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GrantStreakFreezeCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            profile.GrantStreakFreeze(request.Count);
            await _context.SaveChangesAsync(cancellationToken);

            return profile.StreakFreezes;
        }
    }
}