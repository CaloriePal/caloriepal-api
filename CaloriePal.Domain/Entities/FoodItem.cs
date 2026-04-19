namespace CaloriePal.Domain.Entities
{
    public class FoodItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;

        public decimal CaloriesPer100g { get; private set; }

        public decimal ProteinPer100g { get; private set; }
        public decimal CarbsPer100g { get; private set; }
        public decimal FatPer100g { get; private set; }

        protected FoodItem()
        { }

        public static FoodItem Create(
            string name, decimal calories, decimal protein, decimal carbs, decimal fat, bool isCustom = false, Guid? createdByPlayerId = null)
        {
            return new FoodItem
            {
                Name = name,
                CaloriesPer100g = calories,
                ProteinPer100g = protein,
                CarbsPer100g = carbs,
                FatPer100g = fat,
            };
        }
    }
}