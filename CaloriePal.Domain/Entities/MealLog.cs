namespace CaloriePal.Domain.Entities
{
    public class MealLog
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid PlayerId { get; private set; }
        public Guid? FoodItemId { get; private set; }
        public string FoodName { get; private set; } = string.Empty;
        public MealCategory Category { get; private set; }
        public decimal Calories { get; private set; }
        public decimal Protein { get; private set; }
        public decimal Carbs { get; private set; }
        public decimal Fat { get; private set; }
        public DateOnly LoggedOnDate { get; private set; }
        public DateTime LoggedAt { get; private set; }

        protected MealLog()
        { }

        public static MealLog FromFood(
            Guid playerId, FoodItem food,
            MealCategory category, decimal grams)
        {
            var f = grams / 100m;
            return new MealLog
            {
                PlayerId = playerId,
                FoodItemId = food.Id,
                FoodName = food.Name,
                Category = category,
                Calories = Math.Round(food.CaloriesPer100g * f, 1),
                Protein = Math.Round(food.ProteinPer100g * f, 1),
                Carbs = Math.Round(food.CarbsPer100g * f, 1),
                Fat = Math.Round(food.FatPer100g * f, 1),
                LoggedOnDate = DateOnly.FromDateTime(DateTime.UtcNow),
                LoggedAt = DateTime.UtcNow,
            };
        }

        public static MealLog FromManual(
            Guid playerId, string name,
            MealCategory category,
            decimal calories, decimal protein, decimal carbs, decimal fat) =>
            new()
            {
                PlayerId = playerId,
                FoodItemId = null,
                FoodName = name,
                Category = category,
                Calories = calories,
                Protein = protein,
                Carbs = carbs,
                Fat = fat,
                LoggedOnDate = DateOnly.FromDateTime(DateTime.UtcNow),
                LoggedAt = DateTime.UtcNow,
            };
    }

    public enum MealCategory
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack
    }
}