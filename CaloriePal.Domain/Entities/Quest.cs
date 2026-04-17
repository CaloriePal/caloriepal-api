namespace CaloriePal.Domain.Entities
{
    public class Quest
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public QuestCategory Category { get; private set; }
        public QuestType Type { get; private set; }
        public int XpReward { get; private set; }
        public int CoinReward { get; private set; }
        public bool IsActive { get; private set; }
        public int SortOrder { get; private set; }

        private Quest()
        { }

        public static Quest Create(
            string title,
            string description,
            QuestCategory category,
            QuestType type,
            int xpReward,
            int coinReward,
            int sortOrder = 0)
        {
            return new Quest
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Category = category,
                Type = type,
                XpReward = xpReward,
                CoinReward = coinReward,
                IsActive = true,
                SortOrder = sortOrder
            };
        }
    }

    public enum QuestCategory
    { Training, Nutrition, Mindset }

    public enum QuestType
    { Daily }
}