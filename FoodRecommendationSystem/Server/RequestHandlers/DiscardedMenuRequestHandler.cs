using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;

namespace Server.RequestHandlers
{
    public class DiscardedMenuRequestHandler
    {
        private readonly IRecommendationHelper _helper;
        private readonly IFeedbackHelper _feedbackHelper;

        public DiscardedMenuRequestHandler(IRecommendationHelper helper, IFeedbackHelper feedbackHelper)
        {
            _helper = helper;
            _feedbackHelper = feedbackHelper;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"DISCARD_GET",  GetDiscardedMeals },
                {"DISCARD_MENU", GetDiscardMenu },
                {"DISCARD_FEEDBACK", AddDiscardFeedback },
                {"DISCARD_UPDATE" , UpdateDiscardMenu}
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

            result += ($"\n\n\nID: {meals.Id} \t Discarded Meals: {meals.MealName.MealName}");

            return result;
        }

        private string UpdateDiscardMenu(string request)
        {
            var parts = request.Split('|');
            int Id = Convert.ToInt32(parts[1]);
            bool isDiscard = (parts[2] == "2") ? true : false;

            _helper.UpdateDiscardMeal(Id, isDiscard);

            return "Successfull";
        }

        private string GetDiscardMenu(string request)
        {
            var result = _helper.GetDiscardedMenu();

            return ($"{result.Id}|{result.MealName.MealName}");
        }

        private string AddDiscardFeedback(string request)
        {
            var parts = request.Split('|');
            var feedbackDTO = JsonConvert.DeserializeObject<DiscardedMenuFeedbackDTO>(parts[1]);

            _feedbackHelper.AddDiscardedFeedback(feedbackDTO);

            return "Added feedback";
        }
    }
}
