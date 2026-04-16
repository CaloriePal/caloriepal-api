using CaloriePal.Application.Interfaces;
using CaloriePal.Domain.Entities;
using CaloriePal.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.XpEvents.AddXp
{
    public sealed class AddXpCommandHandler : IRequestHandler<AddXpCommand, AddXpResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILevelingService _levelingService;

        public AddXpCommandHandler(IApplicationDbContext context, ILevelingService levelingService)
        {
            _context = context;
            _levelingService = levelingService;
        }

        public async Task<AddXpResult> Handle(AddXpCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken)
                ?? throw new InvalidOperationException($"PlayerProfile not found for user {request.UserId}");

            int levelsGained = profile.AddXp(request.Amount, _levelingService);

            var xpEvent = XpEvent.Create(profile.Id, request.Amount, request.Source);
            _context.XpEvents.Add(xpEvent);

            await _context.SaveChangesAsync(cancellationToken);

            return new AddXpResult(
                NewTotalXp: profile.TotalXp,
                NewLevel: profile.Level,
                XpIntoCurrentLevel: profile.XpIntoCurrentLevel,
                XpRequiredForNextLevel: _levelingService.XpRequiredForNextLevel(profile.Level),
                LevelsGained: levelsGained
            );
        }
    }
}