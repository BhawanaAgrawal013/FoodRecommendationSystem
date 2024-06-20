namespace DataAcessLayer.ModelDTOs
{
    public class MealMenuDTO
    {
        public int Id { get; set; }

        public int NumberOfVotes { get; set; }

        public bool WasPrepared { get; set; }

        public string Classification { get; set; }

        public DateTime CreationDate { get; set; }

        public MealNameDTO MealName { get; set; }

        public static implicit operator MealMenuDTO(MealMenu mealMenu)
        {
            if (mealMenu == null) return null;

            return new MealMenuDTO()
            {
                Id = mealMenu.Id,
                NumberOfVotes = mealMenu.NumberOfVotes,
                WasPrepared = mealMenu.WasPrepared,
                Classification = mealMenu.Classification,
                CreationDate = mealMenu.CreationDate,
                MealName = (MealNameDTO)mealMenu.MealName
            };
        }

        public static implicit operator MealMenu(MealMenuDTO mealMenuDTO)
        {
            if (mealMenuDTO == null) return null;

            return new MealMenu()
            {
                Id = mealMenuDTO.Id,
                NumberOfVotes = mealMenuDTO .NumberOfVotes,
                WasPrepared = mealMenuDTO .WasPrepared,
                Classification = mealMenuDTO.Classification,
                MealNameId = mealMenuDTO.MealName.MealNameId,
                CreationDate = mealMenuDTO.CreationDate
            };
        }
    }
}
