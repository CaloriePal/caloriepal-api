using CaloriePal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Players.GetActivityLog
{
    public sealed class GetActivityLogQueryHandler : IRequestHandler<GetActivityLogQuery, List<ActivityLogEntryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetActivityLogQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityLogEntryDto>> Handle(GetActivityLogQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException("PlayerProfile not found.");

            var rows = await _context.PlayerQuestLogs
                .Include(l => l.Quest)
                .Where(l => l.PlayerProfileId == profile.Id)
                .OrderByDescending(l => l.CompletedAt)
                .Take(5)
                .ToListAsync(cancellationToken);

            return rows.Select(l => new ActivityLogEntryDto(
                l.Id,
                l.Quest.Title,
                l.Quest.Category.ToString(),
                l.Quest.XpReward,
                l.CompletedAt
            )).ToList();
        }
    }
}