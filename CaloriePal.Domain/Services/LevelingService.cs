namespace CaloriePal.Domain.Services
{
    public sealed class LevelingService : ILevelingService
    {
        private const double Exponent = 1.5;
        private const int BaseXp = 100;

        public (int Level, int XpIntoCurrentLevel) CalculateLevel(int totalXp)
        {
            if (totalXp < 0) totalXp = 0;

            int level = 1;
            while (true)
            {
                int xpNeededForNext = XpRequiredForNextLevel(level);
                int xpThreshold = TotalXpForLevel(level + 1);

                if (totalXp < xpThreshold)
                {
                    int xpIntoLevel = totalXp - TotalXpForLevel(level);
                    return (level, xpIntoLevel);
                }

                level++;
            }
        }

        public int TotalXpForLevel(int level)
        {
            if (level <= 1) return 0;
            int total = 0;
            for (int i = 1; i < level; i++)
                total += XpRequiredForNextLevel(i);
            return total;
        }

        public int XpRequiredForNextLevel(int currentLevel)
        {
            return (int)(BaseXp * Math.Pow(currentLevel, Exponent));
        }
    }
}