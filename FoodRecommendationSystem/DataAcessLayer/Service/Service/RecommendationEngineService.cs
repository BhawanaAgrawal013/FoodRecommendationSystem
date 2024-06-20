namespace DataAcessLayer.Service.Service
{
    public class RecommendationEngineService : IRecommendationEngineService
    {
        private readonly ISummaryRatingService _summaryRatingService;
        private readonly IMealMenuService _mealMenuService;
        private readonly IMealService _mealService;

        public RecommendationEngineService(ISummaryRatingService summaryRatingService, IMealMenuService mealMenuService, IMealService mealService)
        {
            _summaryRatingService = summaryRatingService;
            _mealMenuService = mealMenuService;
            _mealService = mealService;
        }

        public List<RecommendedMeal> GiveRecommendation(string classification, int numberOfMeals)
        {
            var recommendedMeals = new List<RecommendedMeal>();

            var sortingCriteria = new List<(Func<SummaryRatingDTO, double> selector, int takeCount)>
            {
                (x => x.SentimentScore, 20),
                (x => x.AverageRating, 20),
                (x => x.TotalQualityRating, 20),
                (x => x.TotalQuantityRating, 20),
                (x => x.TotalAppearanceRating, 20),
                (x => x.TotalValueForMoneyRating, 20)
            };

            foreach (var (selector, takeCount) in sortingCriteria)
            {
                var summaryRatings = _summaryRatingService.GetAllSummaryRatings()
                    .OrderByDescending(selector)
                    .Take(takeCount);

                var meals = GetRecommendedMeals(summaryRatings, classification);
                recommendedMeals.AddRange(meals);

                if (recommendedMeals.Count >= 20)
                    break;
            }

            return recommendedMeals.Take(numberOfMeals).ToList();
        }


        private List<RecommendedMeal> GetRecommendedMeals(IEnumerable<SummaryRatingDTO> summaryRatings, string classification)
        {
            var meals = _mealService.GetAllMeals();

            List<RecommendedMeal> recommendedMeals = new List<RecommendedMeal>();

            foreach (var summaryRating in summaryRatings)
            {
                var recommendedMeal = meals.Where(x => x.Food.Id == summaryRating.Food.Id && x.MealName.MealType == classification)
                                    .Select(x => new RecommendedMeal
                                    {
                                        MealName = x.MealName,
                                        PrimaryFoodName = x.Food.Name,
                                        SummaryRating = summaryRating
                                    });

                recommendedMeals.AddRange(recommendedMeal);
            }

            recommendedMeals = recommendedMeals.DistinctBy(x => x.MealName.MealNameId).ToList();

            var date = DateTime.Now.AddDays(-7).Date;

            var previousMealMenu = _mealMenuService.GetAllMealMenus().Where(x => x.CreationDate >= (DateTime.Now.AddDays(-7).Date) && !x.WasPrepared).Select(x => x.MealName.MealNameId);

            recommendedMeals = recommendedMeals.Where(id => !previousMealMenu.Contains(id.MealName.MealNameId)).ToList();

            return recommendedMeals;
        }
    }
}