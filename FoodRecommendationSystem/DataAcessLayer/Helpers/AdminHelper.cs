namespace DataAcessLayer.Helpers
{
    public class AdminHelper : IAdminHelper
    {
        private readonly IMealService _mealService;
        private readonly IMealNameService _mealNameService;
        private readonly IFoodService _foodService;

        public AdminHelper(IMealService mealService, IMealNameService mealNameService, IFoodService foodService)
        {
            _mealService = mealService;
            _mealNameService = mealNameService;
            _foodService = foodService;
        }

        public List<MealNameDTO> GetFullMenu()
        {
            return _mealNameService.GetAllMeals();
        }

        public MealNameDTO AddMenuItem(MealNameDTO mealNameDTO)
        {
            var existingFood = _foodService.GetAllFoods().Where(x => x.Name == mealNameDTO.MealName).FirstOrDefault();

            if (existingFood == null)
            {
                FoodDTO foodDTO = new FoodDTO
                {
                    Name = mealNameDTO.MealName,
                    IsAvailable = true,
                };

                _foodService.AddFood(foodDTO);
            }

            var existingMealName = _mealNameService.GetAllMeals().Where(x => x.MealName == mealNameDTO.MealName).FirstOrDefault();

            if (existingMealName == null)
            {
                _mealNameService.AddMealName(mealNameDTO);
            }

            MealDTO mealDTO = new MealDTO();

            mealDTO.Food = _foodService.GetAllFoods().Where(x => x.Name == mealNameDTO.MealName).FirstOrDefault();
            mealDTO.MealName = _mealNameService.GetAllMeals().Where(x => x.MealName == mealNameDTO.MealName).FirstOrDefault();

            var existingMeal = _mealService.GetAllMeals().Where(x => x.Food.Id == mealDTO.Food.Id && x.MealName.MealNameId == mealDTO.MealName.MealNameId).FirstOrDefault();

            if (existingMeal == null)
            {
                _mealService.AddMeal(mealDTO);
            }

            return mealNameDTO;
        }

        public MealNameDTO UpdateMenuItem(MealNameDTO mealDTO)
        {
            _mealNameService.UpdateMealName(mealDTO);
            return mealDTO;
        }

        public string DeleteMenuItem(int mealNameId)
        {
            var existingMeal = _mealNameService.GetAllMeals().Where(x => x.MealNameId == mealNameId).FirstOrDefault();

            if(existingMeal == null)
            {
                return "Meal does not exist";
            }
            else
            {
                _mealService.DeleteMeal(mealNameId);

                return "Meal deleted sucessfully";
            }
        }
    }
}
