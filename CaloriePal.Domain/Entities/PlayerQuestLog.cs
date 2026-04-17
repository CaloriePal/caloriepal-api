namespace CaloriePal.Domain.Entities
{
    public class PlayerQuestLog
    {
        public Guid Id { get; private set; }
        public Guid PlayerProfileId { get; private set; }
        public Guid QuestId { get; private set; }
        public DateTime CompletedAt { get; private set; }
        public DateOnly CompletedOnDate { get; private set; }

        // Navigation — useful for EF includes
        public Quest Quest { get; private set; } = default!;

        private PlayerQuestLog()
        { }

        public static PlayerQuestLog Create(Guid playerProfileId, Guid questId, DateTime completedAt)
        {
            return new PlayerQuestLog
            {
                Id = Guid.NewGuid(),
                PlayerProfileId = playerProfileId,
                QuestId = questId,
                CompletedAt = completedAt,
                CompletedOnDate = DateOnly.FromDateTime(completedAt)
            };
        }
    }
}