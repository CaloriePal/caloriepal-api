namespace CaloriePal.Domain.Services
{
    public sealed class TitleService : ITitleService
    {
        private static readonly (int MinLevel, string Title)[] Tiers =
        [
            (1,  "Rookie Warrior"),
            (3,  "Iron Vanguard"),
            (6,  "Steel Sentinel"),
            (10, "Bronze Titan"),
            (15, "Silver Enforcer"),
            (20, "Gold Gladiator"),
            (30, "Platinum Destroyer"),
            (40, "Diamond Warlord"),
            (50, "Legendary Champion"),
        ];

        public string GetTitle(int level)
        {
            string title = Tiers[0].Title;
            foreach (var (minLevel, name) in Tiers)
            {
                if (level >= minLevel) title = name;
                else break;
            }
            return title;
        }
    }
}