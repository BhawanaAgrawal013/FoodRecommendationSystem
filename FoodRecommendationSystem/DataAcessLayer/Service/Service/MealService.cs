using DataAcessLayer.Entity;
using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class MealService : IMealService
    {
        private readonly IRepository<Food> _foodRepository;
        private readonly IRepository<MealName> _mealNameRepository;
        private readonly IRepository<Meal> _mealRepository;

        public MealService(
            IRepository<Food> foodRepository,
            IRepository<MealName> mealNameRepository,
            IRepository<Meal> mealRepository)
        {
            _foodRepository = foodRepository;
            _mealNameRepository = mealNameRepository;
            _mealRepository = mealRepository;
        }

        public int AddMeal(MealDTO mealDTO)
        {
            try
            {
                var food = _foodRepository.GetAll()
                    .FirstOrDefault(f => f.Name == mealDTO.Food.Name);
                if (food == null)
                {
                    food = (Food)mealDTO.Food;
                    _foodRepository.Insert(food);
                    _foodRepository.Save();
                }

                var mealName = _mealNameRepository.GetAll()
                    .FirstOrDefault(mn => mn.Name == mealDTO.MealName.MealName);
                if (mealName == null)
                {
                    mealName = (MealName)mealDTO.MealName;
                    _mealNameRepository.Insert(mealName);
                    _mealNameRepository.Save();
                }

                var meal = new Meal
                {
                    FoodId = food.Id,
                    MealNameId = mealName.Id
                };

                _mealRepository.Insert(meal);
                _mealRepository.Save();

                return meal.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding the meal", ex);
            }
        }

        public void UpdateMeal(MealDTO mealDTO)
        {
            try
            {
                var meal = _mealRepository.GetById(mealDTO.Id);
                if (meal == null)
                {
                    throw new Exception("Meal not found");
                }

                _mealRepository.Update(meal);
                _mealRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the meal", ex);
            }
        }

        public void DeleteMeal(int id)
        {
            try
            {
                _mealRepository.Delete(id);
                _mealRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the meal with ID {id}", ex);
            }
        }

        public List<MealDTO> GetAllMeals()
        {
            try
            {
                var meals = _mealRepository.GetAll();
                return meals.Select(meal => (MealDTO)meal).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all meals", ex);
            }
        }

        public MealDTO GetMealById(int id)
        {
            try
            {
                var meal = _mealRepository.GetById(id);
                if (meal == null)
                {
                    throw new Exception($"Meal with ID {id} not found");
                }

                return (MealDTO)meal;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the meal with ID {id}", ex);
            }
        }
    }

}
