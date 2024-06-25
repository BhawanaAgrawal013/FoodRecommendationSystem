using DataAcessLayer.Helpers.IHelpers;

namespace Server.RequestHandlers
{
    public class DiscardedMenuRequestHandler
    {
        private readonly IRecommendationHelper _helper;

        public DiscardedMenuRequestHandler(IRecommendationHelper helper)
        {
            _helper = helper;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"DISCARD_GET",  GetDiscardedMeals }
            };

            foreach (var handlerKey in requestHandlers.Keys)
            {
                if (request.StartsWith(handlerKey))
                {
                    var handler = requestHandlers[handlerKey];
                    return handler(request);
                }
            }

            return "Invalid request.";
        }

        private string GetDiscardedMeals(string request)
        {
            var recommendedMeals = _helper.GetDiscardedMeals();

            string result = "";

            foreach (var meal in recommendedMeals)
            {
                result += ($"\nMeal: {meal.MealName.MealName} \t Average Rating: {meal.SummaryRating.AverageRating} " +
                    $"\n Sentiments: {meal.SummaryRating.SentimentComment}\n");
            }

            var meals = _helper.AddDiscardedMeal(recommendedMeals);

            result += ($"\n\n\nDiscarded Meals: {meals.MealName.MealName}");

            return result;
        }
    }
}
