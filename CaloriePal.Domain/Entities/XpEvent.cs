namespace CaloriePal.Domain.Entities
{
    public class XpEvent
    {
        public Guid Id { get; private set; }
        public Guid PlayerProfileId { get; private set; }
        public int Amount { get; private set; }
        public string Source { get; private set; } = null!;
        public DateTime OccurredAt { get; private set; }

        private XpEvent()
        { }

        public static XpEvent Create(Guid playerProfileId, int amount, string source)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

            return new XpEvent
            {
                Id = Guid.NewGuid(),
                PlayerProfileId = playerProfileId,
                Amount = amount,
                Source = source,
                OccurredAt = DateTime.UtcNow
            };
        }
    }
}