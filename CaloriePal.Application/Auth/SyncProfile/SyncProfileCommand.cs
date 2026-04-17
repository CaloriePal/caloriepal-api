using MediatR;

namespace CaloriePal.Application.Auth.SyncProfile
{
    public record SyncProfileCommand(
        string DisplayName,
        string? AvatarUrl
    ) : IRequest<SyncProfileResult>;

    public record SyncProfileResult(
        Guid Id,
        Guid UserId,
        string DisplayName,
        int Level,
        int TotalXp,
        bool IsNewUser
    );
}