namespace DataAcessLayer.ModelDTOs
{
    public class MealNameDTO
    {
        public int MealNameId { get; set; }

        public string MealName { get; set; }

        public string MealType { get; set; }

        public bool IsDeleted {  get; set; }

        public static implicit operator MealNameDTO(MealName mealName)
        {
            if (mealName == null) return null;

            return new MealNameDTO()
            {
                MealNameId = mealName.Id,
                MealName = mealName.Name,
                MealType = mealName.MealType,
                IsDeleted = mealName.IsDeleted
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
                IsDeleted = mealNameDTO.IsDeleted
            };
        }
    }
}
