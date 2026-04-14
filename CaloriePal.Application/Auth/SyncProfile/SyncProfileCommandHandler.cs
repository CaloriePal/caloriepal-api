using CaloriePal.Application.Interfaces;
using CaloriePal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CaloriePal.Application.Auth.SyncProfile
{
    public class SyncProfileCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    ) : IRequestHandler<SyncProfileCommand, SyncProfileResult>
    {
        public async Task<SyncProfileResult> Handle(SyncProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId
                         ?? throw new UnauthorizedAccessException("User is not authenticated.");

            var profile = await context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);

            bool isNewUser = profile is null;

            if (isNewUser)
            {
                profile = PlayerProfile.Create(userId, request.Email, request.DisplayName, request.AvatarUrl);
                await context.PlayerProfiles.AddAsync(profile, cancellationToken);
            }
            else
            {
                profile.UpdateProfile(request.DisplayName, request.AvatarUrl);
            }

            await context.SaveChangesAsync(cancellationToken);

            return new SyncProfileResult(
                UserId: profile.Id,
                DisplayName: profile.DisplayName,
                Level: profile.Level,
                TotalXp: profile.TotalXp,
                Coins: profile.Coins,
                IsNewUser: isNewUser
            );
        }
    }
}