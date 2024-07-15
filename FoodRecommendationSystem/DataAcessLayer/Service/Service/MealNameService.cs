using Serilog;

namespace DataAcessLayer.Service.Service
{
    public class MealNameService : IMealNameService
    {
        private readonly IRepository<MealName> _mealNameRepository;
        public MealNameService(IRepository<MealName> mealNameRepository)
        {
            _mealNameRepository = mealNameRepository;
        }

        public void AddMealName(MealNameDTO mealNameDTO)
        {
            try
            {
                MealName mealName = (MealName)mealNameDTO;
                _mealNameRepository.Insert(mealName);
                _mealNameRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error inserting meal name: {ex.Message}");
                throw new Exception("Error inserting meal name", ex);
            }
        }

        public List<MealNameDTO> GetAllMeals()
        {
            try
            {
                var mealNames = _mealNameRepository.GetAll();
                return mealNames.Select(mealName => (MealNameDTO)mealName).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting all the meals: {ex.Message}");
                throw new Exception("Error getting all the meals", ex);
            }
        }

        public MealNameDTO GetMealName(int id)
        {
            try
            {
                var mealName = _mealNameRepository.GetById(id);
                return (MealNameDTO)mealName;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the meal {id}: {ex.Message}");
                throw new Exception($"Error getting the meal {id}", ex);
            }
        }

        public void UpdateMealName(MealNameDTO mealNameDTO)
        {
            try
            {
                var mealName = (MealName)mealNameDTO;
                _mealNameRepository.Update(mealName);
                _mealNameRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating the Meal Name: {ex.Message}");
                throw new Exception("Error updating the Meal Name", ex);
            }
        }

        public void DeleteMealName(int id)
        {
            try
            {
                var mealName = _mealNameRepository.GetById(id);
                mealName.IsDeleted = true;
                _mealNameRepository.Update(mealName);
                _mealNameRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting the Meal Name with id {id}: {ex.Message}");
                throw new Exception($"Error deleting the Meal Name with id {id}", ex);
            }
        }

    }
}
