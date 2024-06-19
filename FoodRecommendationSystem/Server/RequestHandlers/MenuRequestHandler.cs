using DataAcessLayer.ModelDTOs;
using DataAcessLayer.Service.IService;
using Newtonsoft.Json;

namespace Server.RequestHandlers
{
    public class MenuRequestHandler
    {
        private readonly IMealNameService _mealNameService;  
        public MenuRequestHandler(IMealNameService mealNameService)
        {
            _mealNameService = mealNameService;  
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                { "MENU_GET", HandleGetMenuRequest },
                { "MENU_ADD", HandleAddMenuNameRequest }
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

        private string HandleGetMenuRequest(string request)
        {
            var mealName = _mealNameService.GetAllMeals();  
            string meals = string.Join(' ', mealName.Select(x => x.MealName));
            return meals;
        }

        private string HandleAddMenuNameRequest(string request)
        {
            var parts = request.Split('|');
            if (parts.Length < 2)
                return "Invalid request format.";

            string jsonMealName = parts[1];
            MealNameDTO mealName = JsonConvert.DeserializeObject<MealNameDTO>(jsonMealName);  

            _mealNameService.AddMealName(mealName);

            return "Meal Added";
        }
    }

}
