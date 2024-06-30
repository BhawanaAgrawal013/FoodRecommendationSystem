namespace DataAcessLayer.ModelDTOs
{
    public class MealDTO
    {
        public int Id { get; set; }

        public FoodDTO Food { get; set; }

        public MealNameDTO MealName { get; set; }

        public bool IsDeleted { get; set; }

        public static implicit operator MealDTO(Meal meal)
        {
            if (meal == null) return null;

            return new MealDTO()
            {
                Id = meal.Id,
                Food = (FoodDTO)meal.Food,
                MealName = (MealNameDTO)meal.MealName,
                IsDeleted = meal.IsDeleted
            };
        }

        public static implicit operator Meal(MealDTO mealDTO)
        {
            if (mealDTO == null) return null;

            return new Meal()
            {
                Id = mealDTO.Id,
                FoodId = mealDTO.Food.Id,
                MealNameId = mealDTO.MealName.MealNameId,
                Food = (Food)mealDTO.Food,
                MealName = (MealName)mealDTO.MealName,
                IsDeleted = (mealDTO.IsDeleted)
            };
        }
    }
}
