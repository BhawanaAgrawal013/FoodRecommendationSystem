namespace DataAcessLayer.Helpers
{
    public class RecommendationHelper : IRecommendationHelper
    {
        private IRecommendationEngineService _recommendationEngineService;
        private IDiscardedMenuService _discardedMenuService;
        private IMealNameService _mealNameService;
        private IMealService _mealService;

        public RecommendationHelper(IRecommendationEngineService recommendationEngineService,
                                    IDiscardedMenuService discardedMenuService, IMealNameService mealNameService
                                , IMealService mealService)
        {
            _recommendationEngineService = recommendationEngineService;
            _discardedMenuService = discardedMenuService;
            _mealNameService = mealNameService;
            _mealService = mealService;
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

            int discardId = _discardedMenuService.GetDiscardedMenuList().Where(x => x.MealNameId == mealName.MealNameId).LastOrDefault().Id;
            recommendedMeal.Id = discardId;

            return recommendedMeal;
        }

        public DiscardedMenuDTO GetDiscardedMenu()
        {
            return _discardedMenuService.GetDiscardedMenuList().Where(x => !x.IsDiscarded).FirstOrDefault();
        }

        public void UpdateDiscardMeal(int discardId, bool isDiscarded)
        {
            var discardedMeal = _discardedMenuService.GetDiscardedMenuList().Where(x => x.Id == discardId).FirstOrDefault();
            discardedMeal.IsDiscarded = isDiscarded;

            if(isDiscarded)
            {
                DeleteDiscardedMenu(discardedMeal);
            }

            _discardedMenuService.UpdateDiscardedMenu(discardedMeal);
        }

        public void DeleteDiscardedMenu(DiscardedMenu discardedMenu)
        {
            var mealName = _mealNameService.GetMealName(discardedMenu.MealNameId);
            mealName.IsDeleted = true;

            _mealNameService.UpdateMealName(mealName);

            var meals = _mealService.GetAllMeals().Where(x => x.MealName.MealNameId == discardedMenu.MealNameId).ToList();

            foreach(var meal in meals)
            {
                meal.IsDeleted = true;
                _mealService.UpdateMeal(meal);
            }
        }
    }
}
