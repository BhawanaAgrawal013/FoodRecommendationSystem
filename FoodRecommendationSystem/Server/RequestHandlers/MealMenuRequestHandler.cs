using DataAcessLayer.Common;
using DataAcessLayer.Service.IService;

namespace Server.RequestHandlers
{
    public class MealMenuRequestHandler
    {
        private readonly IRecommendationEngineService _service;

        public MealMenuRequestHandler(IRecommendationEngineService service)
        {
            _service = service;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"MEAL_SELECT",  GetRecommendedMeals }
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

        private string GetRecommendedMeals(string request)
        {
            var parts = request.Split('|');
            string classification = parts[1];

            var recommendedMeals = _service.GiveRecommendation(classification);

            string result = "";

            foreach (var meal in recommendedMeals)
            {
                result += ($"\nMeal: {meal.MealName.MealName}, Primary Food: {meal.PrimaryFoodName}");
            }

            return result;
        }
    }
}
