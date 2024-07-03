using Serilog;

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
            try
            {
                return _mealNameService.GetAllMeals();
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving full menu: {ex.Message}");
                throw new Exception($"Error retrieving full menu: {ex.Message}");
            }
        }

        public MealNameDTO AddMenuItem(MealNameDTO mealNameDTO)
        {
            try
            {
                var existingFood = _foodService.GetAllFoods().FirstOrDefault(x => x.Name == mealNameDTO.MealName);

                if (existingFood == null)
                {
                    FoodDTO foodDTO = new FoodDTO
                    {
                        Name = mealNameDTO.MealName,
                        IsAvailable = true,
                    };

                    _foodService.AddFood(foodDTO);
                }

                var existingMealName = _mealNameService.GetAllMeals().FirstOrDefault(x => x.MealName == mealNameDTO.MealName);

                if (existingMealName == null)
                {
                    _mealNameService.AddMealName(mealNameDTO);
                }

                MealDTO mealDTO = new MealDTO
                {
                    Food = _foodService.GetAllFoods().FirstOrDefault(x => x.Name == mealNameDTO.MealName),
                    MealName = _mealNameService.GetAllMeals().FirstOrDefault(x => x.MealName == mealNameDTO.MealName)
                };

                var existingMeal = _mealService.GetAllMeals().FirstOrDefault(x => x.Food.Id == mealDTO.Food.Id && x.MealName.MealNameId == mealDTO.MealName.MealNameId);

                if (existingMeal == null)
                {
                    _mealService.AddMeal(mealDTO);
                }

                return mealNameDTO;
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding menu item: {ex.Message}");
                throw new Exception($"Error adding menu item: {ex.Message}");
            }
        }

        public MealNameDTO UpdateMenuItem(MealNameDTO mealDTO)
        {
            try
            {
                _mealNameService.UpdateMealName(mealDTO);
                return mealDTO;
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating menu item: {ex.Message}");
                throw new Exception($"Error updating menu item: {ex.Message}");
            }
        }

        public string DeleteMenuItem(int mealNameId)
        {
            try
            {
                var existingMeal = _mealNameService.GetAllMeals().FirstOrDefault(x => x.MealNameId == mealNameId);

                if (existingMeal == null)
                {
                    return "Meal does not exist";
                }
                else
                {
                    _mealNameService.DeleteMealName(mealNameId);
                    return "Meal deleted successfully";
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting menu item: {ex.Message}");
                throw new Exception($"Error deleting menu item: {ex.Message}");
            }
        }
    }
}
