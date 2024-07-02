using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Serilog;

namespace Server.RequestHandlers
{
    public class DiscardedMenuRequestHandler : IRequestHandler<DiscardedMenuRequestHandler>
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

            return "";
        }

        private string GetDiscardedMeals(string request)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error($"Error getting the discarded meals {ex.Message}");
                throw new Exception($"Error getting the discarded meals {ex.Message}");
            }
        }

        private string UpdateDiscardMenu(string request)
        {
            try
            {
                var parts = request.Split('|');
                int Id = Convert.ToInt32(parts[1]);
                bool isDiscard = (parts[2] == "2") ? true : false;

                _helper.UpdateDiscardMeal(Id, isDiscard);

                return "Successfull";
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating the discarded meals {ex.Message}");
                throw new Exception($"Error updating the discarded meals {ex.Message}");
            }
        }

        private string GetDiscardMenu(string request)
        {
            try
            {
                var result = _helper.GetDiscardedMenu();

                return ($"{result.Id}|{result.MealName.MealName}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the discarded menu {ex.Message}");
                throw new Exception($"Error getting the discarded menu {ex.Message}");
            }
        }

        private string AddDiscardFeedback(string request)
        {
            try
            {
                var parts = request.Split('|');
                var feedbackDTO = JsonConvert.DeserializeObject<DiscardedMenuFeedbackDTO>(parts[1]);

                _feedbackHelper.AddDiscardedFeedback(feedbackDTO);

                return "Added feedback";
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the discarded meals {ex.Message}");
                throw new Exception($"Error getting the discarded menu {ex.Message}");
            }
        }
    }
}
