using MediatR;

namespace CaloriePal.Application.Players.GetActivityLog
{
    public sealed record GetActivityLogQuery(Guid UserId) : IRequest<List<ActivityLogEntryDto>>;

    public sealed record ActivityLogEntryDto(
        Guid LogId,
        string QuestTitle,
        string Category,
        int XpAwarded,
        DateTime CompletedAt
    );
}