using DataAcessLayer.Helpers.IHelpers;
using Newtonsoft.Json;

namespace Server.RequestHandlers
{
    public class MealMenuRequestHandler : IRequestHandler<MealMenuRequestHandler>
    {
        private readonly IRecommendationHelper _recommendationHelper;
        private readonly IChefHelper _chefHelper;
        private readonly IEmployeeHelper _employeeHelper;

        public MealMenuRequestHandler(IRecommendationHelper recommendationHelper, IChefHelper chefHelper, IEmployeeHelper employeeHelper)
        {
            _recommendationHelper = recommendationHelper;
            _chefHelper = chefHelper;
            _employeeHelper = employeeHelper;
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                {"MEAL_SELECT",  GetRecommendedMeals },
                {"MEAL_OPTION", SendMealMenuOptions },
                {"MEAL_GETOPTIONS",  GetMealMenuOptions },
                {"MEAL_VOTE", VoteForMeal },
                {"MEAL_CHOOSE", ChooseMeal }
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
            int numberOfMeals = Convert.ToInt32(parts[2]);

            var recommendedMeals = _recommendationHelper.GiveRecommendation(classification, numberOfMeals);

            string result = "";

            foreach (var meal in recommendedMeals)
            {
                result += ($"\nMeal: {meal.MealName.MealName}, Primary Food: {meal.PrimaryFoodName}");
            }

            return result;
        }

        private string SendMealMenuOptions(string request)
        {
            var parts = request.Split('|');
            string classification = parts[1];

            List<string> meals = JsonConvert.DeserializeObject<List<string>>(parts[2]);

            _chefHelper.CreateNextMealMenu(meals, classification);

            return "Created Meal Options";
        }

        private string GetMealMenuOptions(string request)
        {
            var parts = request.Split('|');
            string classification = parts[1];

            var meals = _employeeHelper.GetMealMenuOption(DateTime.Now.Date, classification, parts[2]);
            string result = "";

            foreach (var meal in meals)
            {
                result += ($"\nID: {meal.Id} Meal: {meal.MealName.MealName} and Vote: {meal.NumberOfVotes}");
            }

            return result;
        }

        private string VoteForMeal(string request)
        {
            var parts = request.Split('|');
            int id = Convert.ToInt32(parts[1]);

            var meal = _employeeHelper.VoteForNextDayMeal(id, DateTime.Now.Date);

            return ($"Selected Meal: {meal.MealName.MealName}");
        }

        private string ChooseMeal(string request)
        {
            var parts = request.Split('|');

            int mealMenuId = Convert.ToInt32(parts[1]);

            var meal = _chefHelper.ChooseNextMealMenu(mealMenuId);
            
            return ($"Choosen meal is: \nMeal: {meal.MealName.MealName} and Id: {meal.MealName.MealNameId}");

        }
    }
}
