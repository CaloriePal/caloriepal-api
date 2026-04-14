using MediatR;

namespace CaloriePal.Application.Auth.SyncProfile
{
    public record SyncProfileCommand(
        string Email,
        string DisplayName,
        string? AvatarUrl
    ) : IRequest<SyncProfileResult>;

    public record SyncProfileResult(
        Guid UserId,
        string DisplayName,
        int Level,
        int TotalXp,
        int Coins,
        bool IsNewUser
    );
}
