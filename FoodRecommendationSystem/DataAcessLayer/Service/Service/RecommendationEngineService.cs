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

        //public List<RecommendedMeal> GiveRecommendation(string classification)
        //{
        //    var summaryRatings = _summaryRatingService.GetAllSummaryRatings().OrderBy(x => x.SentimentScore).Take(20);

        //    List<RecommendedMeal> recommendedMeals = GetRecommendedMeals(summaryRatings, classification);

        //    if (recommendedMeals.Count < 10)
        //    {
        //        summaryRatings = _summaryRatingService.GetAllSummaryRatings().OrderBy(x => x.AverageRating).Take(20);

        //        List<RecommendedMeal> mealsByRating = GetRecommendedMeals(summaryRatings, classification);

        //        recommendedMeals.AddRange(mealsByRating);
        //    }

        //    if (recommendedMeals.Count < 10)
        //    {
        //        summaryRatings = _summaryRatingService.GetAllSummaryRatings().OrderBy(x => x.TotalQualityRating).Take(20);

        //        List<RecommendedMeal> mealsByQuality = GetRecommendedMeals(summaryRatings, classification);

        //        recommendedMeals.AddRange(mealsByQuality);
        //    }

        //    if (recommendedMeals.Count < 10)
        //    {
        //        summaryRatings = _summaryRatingService.GetAllSummaryRatings().OrderBy(x => x.TotalQuantityRating).Take(20);

        //        List<RecommendedMeal> mealsByQuantity = GetRecommendedMeals(summaryRatings, classification);

        //        recommendedMeals.AddRange(mealsByQuantity);
        //    }

        //    if (recommendedMeals.Count < 10)
        //    {
        //        summaryRatings = _summaryRatingService.GetAllSummaryRatings().OrderBy(x => x.TotalAppearanceRating).Take(20);

        //        List<RecommendedMeal> mealsByAppearance = GetRecommendedMeals(summaryRatings, classification);

        //        recommendedMeals.AddRange(mealsByAppearance);
        //    }

        //    if (recommendedMeals.Count < 10)
        //    {
        //        summaryRatings = _summaryRatingService.GetAllSummaryRatings().OrderBy(x => x.TotalValueForMoneyRating).Take(20);

        //        List<RecommendedMeal> mealsByVFM = GetRecommendedMeals(summaryRatings, classification);

        //        recommendedMeals.AddRange(mealsByVFM);
        //    }

        //    return recommendedMeals;
        //}

        public List<RecommendedMeal> GiveRecommendation(string classification)
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

                if (recommendedMeals.Count >= 10)
                    break;
            }

            return recommendedMeals.Take(5).ToList();
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

            var previousMealMenu = _mealMenuService.GetAllMealMenus().Where(x => x.CreationDate >= (DateTime.Now.AddDays(-7).Date)).Select(x => x.MealName.MealNameId);

            recommendedMeals = recommendedMeals.Where(id => !previousMealMenu.Contains(id.MealName.MealNameId)).ToList();

            return recommendedMeals;
        }
    }
}