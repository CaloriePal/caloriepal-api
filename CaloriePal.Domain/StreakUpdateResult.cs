namespace CaloriePal.Domain
{
    public sealed class StreakUpdateResult
    {
        public StreakUpdateResultType Type { get; }
        public int? LostStreakLength { get; }

        private StreakUpdateResult(StreakUpdateResultType type, int? lostStreakLength = null)
        {
            Type = type;
            LostStreakLength = lostStreakLength;
        }

        public static readonly StreakUpdateResult AlreadyLoggedToday = new(StreakUpdateResultType.AlreadyLoggedToday);
        public static readonly StreakUpdateResult StreakStarted = new(StreakUpdateResultType.StreakStarted);
        public static readonly StreakUpdateResult StreakExtended = new(StreakUpdateResultType.StreakExtended);
        public static readonly StreakUpdateResult FreezedConsumed = new(StreakUpdateResultType.FreezeConsumed);

        public static StreakUpdateResult StreakBroken(int lostLength) =>
            new(StreakUpdateResultType.StreakBroken, lostLength);

        public bool IsSuccess => Type != StreakUpdateResultType.StreakBroken;
    }

    public enum StreakUpdateResultType
    {
        AlreadyLoggedToday,
        StreakStarted,
        StreakExtended,
        FreezeConsumed,
        StreakBroken
    }
}