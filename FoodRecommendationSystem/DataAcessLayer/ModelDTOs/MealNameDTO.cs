namespace DataAcessLayer.ModelDTOs
{
    public class MealNameDTO
    {
        public int MealNameId { get; set; }

        public string MealName { get; set; }

        public string MealType { get; set; }

        public string DietType { get; set; }

        public string SpiceLevel { get; set; }

        public string CuisinePreference { get; set; }

        public bool IsSweet { get; set; }

        public bool IsDeleted { get; set; }

        public static implicit operator MealNameDTO(MealName mealName)
        {
            if (mealName == null) return null;

            return new MealNameDTO()
            {
                MealNameId = mealName.Id,
                MealName = mealName.Name,
                MealType = mealName.MealType,
                IsDeleted = mealName.IsDeleted,
                DietType = mealName.DietType,
                SpiceLevel = mealName.SpiceLevel,
                CuisinePreference = mealName.CuisinePreference,
                IsSweet = mealName.IsSweet
            };
        }

        public static implicit operator MealName(MealNameDTO mealNameDTO)
        {
            if (mealNameDTO == null) return null;

            return new MealName()
            {
                Id = mealNameDTO.MealNameId,
                Name = mealNameDTO.MealName,
                MealType = mealNameDTO.MealType,
                IsDeleted = mealNameDTO.IsDeleted,
                DietType = mealNameDTO.DietType,
                SpiceLevel = mealNameDTO.SpiceLevel,
                CuisinePreference = mealNameDTO.CuisinePreference,
                IsSweet = mealNameDTO.IsSweet
            };
        }
    }
}
