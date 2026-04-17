using CaloriePal.Domain.Services;

namespace CaloriePal.Domain.Entities
{
    public class PlayerProfile
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string DisplayName { get; private set; } = null!;
        public string? AvatarUrl { get; private set; }

        public int TotalXp { get; private set; }
        public int Level { get; private set; }
        public int XpIntoCurrentLevel { get; private set; }

        public int Coins { get; private set; }

        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }
        public DateOnly? LastActivityDate { get; private set; }
        public int StreakFreezes { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private PlayerProfile()
        { }

        public static PlayerProfile Create(Guid userId, string displayName, string? avatarUrl = null)
        {
            return new PlayerProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DisplayName = displayName,
                AvatarUrl = avatarUrl,
                Level = 1,
                TotalXp = 0,
                XpIntoCurrentLevel = 0,
                Coins = 0,
                CurrentStreak = 0,
                LongestStreak = 0,
                StreakFreezes = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void Update(string displayName, string? avatarUrl)
        {
            DisplayName = displayName;
            AvatarUrl = avatarUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public int AddXp(int amount, ILevelingService levelingService)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

            int previousLevel = Level;
            TotalXp += amount;

            (Level, XpIntoCurrentLevel) = levelingService.CalculateLevel(TotalXp);

            UpdatedAt = DateTime.UtcNow;
            return Level - previousLevel;
        }

        public void AddCoins(int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
            Coins += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SpendCoins(int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
            if (Coins < amount)
                throw new InvalidOperationException("Insufficient coins.");
            Coins -= amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public StreakUpdateResult UpdateStreak(DateOnly today)
        {
            if (LastActivityDate == today)
                return StreakUpdateResult.AlreadyLoggedToday;

            if (LastActivityDate == null)
            {
                // First ever activity
                CurrentStreak = 1;
                LongestStreak = 1;
                LastActivityDate = today;
                UpdatedAt = DateTime.UtcNow;
                return StreakUpdateResult.StreakStarted;
            }

            int daysSinceLast = today.DayNumber - LastActivityDate.Value.DayNumber;

            if (daysSinceLast == 1)
            {
                // Consecutive day
                CurrentStreak++;
                if (CurrentStreak > LongestStreak)
                    LongestStreak = CurrentStreak;
                LastActivityDate = today;
                UpdatedAt = DateTime.UtcNow;
                return StreakUpdateResult.StreakExtended;
            }

            if (daysSinceLast == 2 && StreakFreezes > 0)
            {
                // One day gap — consume freeze
                StreakFreezes--;
                CurrentStreak++;
                if (CurrentStreak > LongestStreak)
                    LongestStreak = CurrentStreak;
                LastActivityDate = today;
                UpdatedAt = DateTime.UtcNow;
                return StreakUpdateResult.FreezedConsumed;
            }

            // Streak broken
            int lostStreak = CurrentStreak;
            CurrentStreak = 1;
            LastActivityDate = today;
            UpdatedAt = DateTime.UtcNow;
            return StreakUpdateResult.StreakBroken(lostStreak);
        }

        public void GrantStreakFreeze(int count = 1)
        {
            StreakFreezes += count;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}