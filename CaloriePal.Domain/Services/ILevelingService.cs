namespace CaloriePal.Domain.Services
{
    public interface ILevelingService
    {
        (int Level, int XpIntoCurrentLevel) CalculateLevel(int totalXp);

        int TotalXpForLevel(int level);

        int XpRequiredForNextLevel(int currentLevel);
    }
}