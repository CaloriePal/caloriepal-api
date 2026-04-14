namespace CaloriePal.Domain.Entities
{
    public class PlayerProfile
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string DisplayName { get; private set; } = null!;
        public string? AvatarUrl { get; private set; }
        public int Level { get; private set; }
        public int TotalXp { get; private set; }
        public int Coins { get; private set; }
        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }
        public DateTime? LastActivityDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private PlayerProfile() { }

        public static PlayerProfile Create(Guid supabaseUserId, string email, string displayName, string? avatarUrl = null)
        {
            return new PlayerProfile
            {
                Id = supabaseUserId,
                Email = email,
                DisplayName = displayName,
                AvatarUrl = avatarUrl,
                Level = 1,
                TotalXp = 0,
                Coins = 0,
                CurrentStreak = 0,
                LongestStreak = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void UpdateProfile(string displayName, string? avatarUrl)
        {
            DisplayName = displayName;
            AvatarUrl = avatarUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
