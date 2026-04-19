using MediatR;

namespace CaloriePal.Application.XpEvents.AddXp
{
    public sealed record AddXpCommand(
        Guid UserId,
        int Amount,
        string Source
    ) : IRequest<AddXpResult>;

    public sealed record AddXpResult(
        int NewTotalXp,
        int NewLevel,
        int XpIntoCurrentLevel,
        int XpRequiredForNextLevel,
        int LevelsGained
    );

    public sealed record AddXpRequest(int Amount, string Source);
}