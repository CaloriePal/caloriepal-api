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
            var userId = currentUser.UserId;

            var profile = await context.PlayerProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

            bool isNewUser = profile is null;

            if (isNewUser)
            {
                profile = PlayerProfile.Create(userId, request.DisplayName, request.AvatarUrl);
                await context.PlayerProfiles.AddAsync(profile, cancellationToken);
            }
            else
            {
                profile!.Update(request.DisplayName, request.AvatarUrl);
            }

            await context.SaveChangesAsync(cancellationToken);

            return new SyncProfileResult(
                Id: profile!.Id,
                UserId: profile!.UserId,
                DisplayName: profile.DisplayName,
                Level: profile.Level,
                TotalXp: profile.TotalXp,
                IsNewUser: isNewUser
            );
        }
    }
}