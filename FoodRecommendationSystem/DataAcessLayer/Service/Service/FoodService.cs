using DataAcessLayer.Service.IService;
using Serilog;

namespace DataAcessLayer.Service.Service
{
    public class FoodService : IFoodService
    {
        private readonly IRepository<Food> _foodRepository;
        public FoodService(IRepository<Food> foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public int AddFood(FoodDTO foodDTO)
        {
            try
            {
                Food food = (Food)foodDTO;
                _foodRepository.Insert(food);
                _foodRepository.Save();

                var foods = _foodRepository.GetAll();
                return foods.FirstOrDefault(x => x.Name == foodDTO.Name)?.Id ?? 0;
            }
            catch (Exception ex)
            {
                Log.Error($"Error inserting food: {ex.Message}");
                throw new Exception("Error inserting food", ex);
            }
        }

        public List<FoodDTO> GetAllFoods()
        {
            try
            {
                var foods = _foodRepository.GetAll();
                return foods.Select(food => (FoodDTO)food).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting all foods: {ex.Message}");
                throw new Exception("Error getting all foods", ex);
            }
        }

        public FoodDTO GetFood(int id)
        {
            try
            {
                var food = _foodRepository.GetById(id);
                return (FoodDTO)food;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the food {id}: {ex.Message}");
                throw new Exception($"Error getting the food {id}", ex);
            }
        }

        public void UpdateFood(FoodDTO foodDTO)
        {
            try
            {
                var food = (Food)foodDTO;
                _foodRepository.Update(food);
                _foodRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating the food: {ex.Message}");
                throw new Exception("Error updating the food", ex);
            }
        }

        public void DeleteFood(int id)
        {
            try
            {
                _foodRepository.Delete(id);
                _foodRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting the food {id}: {ex.Message}");
                throw new Exception($"Error deleting the food {id}", ex);
            }
        }
    }

}
