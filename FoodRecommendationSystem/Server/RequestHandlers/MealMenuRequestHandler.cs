using DataAcessLayer.Helpers.IHelpers;
using Newtonsoft.Json;
using Serilog;
using System.Text;

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

            return "";
        }

        private string GetRecommendedMeals(string request)
        {
            try
            {
                var parts = request.Split('|');
                string classification = parts[1];
                int numberOfMeals = Convert.ToInt32(parts[2]);

                Log.Information("Getting the recommendation for {classification}", classification);

                var recommendedMeals = _recommendationHelper.GiveRecommendation(classification, numberOfMeals);

                string result = "";

                foreach (var meal in recommendedMeals)
                {
                    result += ($"\nMeal: {meal.MealName.MealName}, Primary Food: {meal.PrimaryFoodName}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the recommended meal {ex.Message}");
                throw new Exception($"Error getting the recommended meal {ex.Message}");
            }
        }

        private string SendMealMenuOptions(string request)
        {
            try
            {
                var parts = request.Split('|');
                string classification = parts[1];

                List<string> meals = JsonConvert.DeserializeObject<List<string>>(parts[2]);

                Log.Information("Rolled out meal options for {class}", classification);
                _chefHelper.CreateNextMealMenu(meals, classification);

                return "Created Meal Options";
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating meal options: {ex.Message}");
                throw new Exception($"Error creating meal options: {ex.Message}");
            }
        }


        private string GetMealMenuOptions(string request)
        {
            try
            {
                var parts = request.Split('|');
                string classification = parts[1];

                Log.Information("Getting the meal options for employee");

                var meals = _employeeHelper.GetMealMenuOption(DateTime.Now.Date, classification, parts[2]);
                StringBuilder result = new StringBuilder();

                foreach (var meal in meals)
                {
                    result.AppendLine($"ID: {meal.Id} Meal: {meal.MealName.MealName} and Vote: {meal.NumberOfVotes}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting meal menu options: {ex.Message}");
                throw new Exception($"Error getting meal menu options: {ex.Message}");
            }
        }


        private string VoteForMeal(string request)
        {
            try
            {
                var parts = request.Split('|');
                int id = Convert.ToInt32(parts[1]);

                Log.Information($"Employee voted for item {id}");

                var meal = _employeeHelper.VoteForNextDayMeal(id, DateTime.Now.Date);

                return $"Selected Meal: {meal.MealName.MealName}";
            }
            catch (Exception ex)
            {
                Log.Error($"Error voting for meal: {ex.Message}");
                throw new Exception($"Error voting for meal: {ex.Message}");
            }
        }


        private string ChooseMeal(string request)
        {
            try
            {
                var parts = request.Split('|');
                int mealMenuId = Convert.ToInt32(parts[1]);

                Log.Information("Choosing the next day final meal");
                var meal = _chefHelper.ChooseNextMealMenu(mealMenuId);

                return $"Chosen meal is: \nMeal: {meal.MealName.MealName} and Id: {meal.MealName.MealNameId}";
            }
            catch (Exception ex)
            {
                Log.Error($"Error choosing meal: {ex.Message}");
                throw new Exception($"Error choosing meal: {ex.Message}");
            }
        }

    }
}
