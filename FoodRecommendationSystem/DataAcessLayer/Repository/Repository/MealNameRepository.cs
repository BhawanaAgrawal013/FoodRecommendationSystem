using DataAcessLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace DataAcessLayer.Repository.Repository
{
    public class MealNameRepository : IMealNameRepository
    {
        private readonly FoodRecommendationContext _dbContext;
        public MealNameRepository(FoodRecommendationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string AddMealName(MealNameDTO mealNameDTO)
        {
            _dbContext.MealNames.Add(mealNameDTO);

            return "Added Meals";
        }

        public List<MealNameDTO> GetAllMeals()
        {
            var mealNames = _dbContext.MealNames.ToList();
            return mealNames.Select(mealName => (MealNameDTO)mealName).ToList();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
