using Serilog;

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

        public List<RecommendedMeal> GiveRecommendation(string classification, int numberOfMeals)
        {
            try
            {
                return _recommendationEngineService.GiveRecommendation(classification, numberOfMeals);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception sending recommendation for {classification}: {ex.Message}");
                throw new Exception($"Exception sending recommendation for {classification}", ex);
            }
        }

        public List<RecommendedMeal> GetDiscardedMeals()
        {
            try
            {
                return _recommendationEngineService.GetDiscardedMeals().Take(3).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Exception retrieving discarded meals: {ex.Message}");
                throw new Exception("Exception retrieving discarded meals", ex);
            }
        }

        public RecommendedMeal AddDiscardedMeal(List<RecommendedMeal> recommendedMeals)
        {
            try
            {
                var recommendedMeal = recommendedMeals
                    .OrderByDescending(x => x.ShouldBeDiscarded)
                    .FirstOrDefault(x => x.SummaryRating.AverageRating < 2);

                if (recommendedMeal == null)
                {
                    throw new Exception("No suitable meal found for discarding");
                }

                var mealName = _mealNameService.GetAllMeals()
                    .FirstOrDefault(x => x.MealName == recommendedMeal.MealName.MealName);

                if (mealName == null)
                {
                    throw new Exception("Meal name not found");
                }

                var discardedMenuDTO = new DiscardedMenuDTO
                {
                    MealNameId = mealName.MealNameId
                };

                _discardedMenuService.AddDiscardedMenu(discardedMenuDTO);

                var discardId = _discardedMenuService.GetDiscardedMenuList()
                    .LastOrDefault(x => x.MealNameId == mealName.MealNameId)?.Id;

                if (discardId == null)
                {
                    throw new Exception("Discarded menu ID not found");
                }

                recommendedMeal.Id = discardId.Value;

                return recommendedMeal;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception adding discarded meal: {ex.Message}");
                throw new Exception("Exception adding discarded meal", ex);
            }
        }

        public DiscardedMenuDTO GetDiscardedMenu()
        {
            try
            {
                return _discardedMenuService.GetDiscardedMenuList().FirstOrDefault(x => !x.IsDiscarded);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception getting discarded menu: {ex.Message}");
                throw new Exception("Exception getting discarded menu", ex);
            }
        }

        public void UpdateDiscardMeal(int discardId, bool isDiscarded)
        {
            try
            {
                var discardedMeal = _discardedMenuService.GetDiscardedMenuList()
                    .FirstOrDefault(x => x.Id == discardId);

                if (discardedMeal == null)
                {
                    throw new Exception("Discarded meal not found");
                }

                discardedMeal.IsDiscarded = isDiscarded;

                if (isDiscarded)
                {
                    DeleteDiscardedMenu(discardedMeal);
                }

                _discardedMenuService.UpdateDiscardedMenu(discardedMeal);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception updating discard meal: {ex.Message}");
                throw new Exception("Exception updating discard meal", ex);
            }
        }

        public void DeleteDiscardedMenu(DiscardedMenu discardedMenu)
        {
            try
            {
                var mealName = _mealNameService.GetMealName(discardedMenu.MealNameId);
                if (mealName == null)
                {
                    throw new Exception("Meal name not found");
                }

                mealName.IsDeleted = true;
                _mealNameService.UpdateMealName(mealName);

                var meals = _mealService.GetAllMeals()
                    .Where(x => x.MealName.MealNameId == discardedMenu.MealNameId)
                    .ToList();

                foreach (var meal in meals)
                {
                    meal.IsDeleted = true;
                    _mealService.UpdateMeal(meal);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Exception deleting discarded menu: {ex.Message}");
                throw new Exception("Exception deleting discarded menu", ex);
            }
        }
    }
}
