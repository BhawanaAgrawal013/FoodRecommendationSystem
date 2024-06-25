namespace DataAcessLayer.Helpers
{
    public class RecommendationHelper : IRecommendationHelper
    {
        private IRecommendationEngineService _recommendationEngineService;
        private IDiscardedMenuService _discardedMenuService;
        private IMealNameService _mealNameService;

        public RecommendationHelper(IRecommendationEngineService recommendationEngineService,
                                    IDiscardedMenuService discardedMenuService, IMealNameService mealNameService)
        {
            _recommendationEngineService = recommendationEngineService;
            _discardedMenuService = discardedMenuService;
            _mealNameService = mealNameService;
        }

        public List<RecommendedMeal> GetDiscardedMeals()
        {
            return _recommendationEngineService.GetDiscardedMeals().Take(3).ToList();
        }

        public RecommendedMeal AddDiscardedMeal(List<RecommendedMeal> recommendedMeals)
        {
            var recommendedMeal = recommendedMeals.OrderByDescending(x => x.ShouldBeDiscarded).Where(x => x.SummaryRating.AverageRating < 2).FirstOrDefault();
            var mealName = _mealNameService.GetAllMeals().Where(x => x.MealName == recommendedMeal.MealName.MealName).FirstOrDefault();

            DiscardedMenuDTO discardedMenuDTO = new DiscardedMenuDTO
            {
                MealNameId = mealName.MealNameId,
            };

            _discardedMenuService.AddDiscardedMenu(discardedMenuDTO);

            return recommendedMeal;
        }
    }
}
