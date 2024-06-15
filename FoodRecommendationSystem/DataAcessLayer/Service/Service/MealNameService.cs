using DataAcessLayer.Repository.IRepository;
using DataAcessLayer.Repository.Repository;
using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class MealNameService : IMealNameService
    {
        private readonly IMealNameRepository _mealNameRepository;
        public MealNameService(IMealNameRepository mealNameRepository)
        {
            _mealNameRepository = mealNameRepository;
        }

        public string AddMealName(MealNameDTO mealNameDTO)
        {
            return _mealNameRepository.AddMealName(mealNameDTO);
        }

        public List<MealNameDTO> GetAllMeals()
        {
            return _mealNameRepository.GetAllMeals();
        }
    }
}
