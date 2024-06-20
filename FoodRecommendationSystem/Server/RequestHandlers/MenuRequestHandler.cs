using DataAcessLayer.Entity;
using DataAcessLayer.ModelDTOs;
using DataAcessLayer.Service.IService;
using Newtonsoft.Json;
using System.Text;

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
                { "MENU_ADD", HandleAddMenuNameRequest },
                { "MENU_BYID", HandleGetMenuByIdRequest },
                { "MENU_UPDATE", HandleUpdateMenuNameRequest },
                { "MENU_DELETE", HandleDeleteMenuRequest }
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
            var mealNames = _mealNameService.GetAllMeals();
            StringBuilder sb = new StringBuilder();

            foreach (var mealName in mealNames)
            {
                sb.AppendLine($"ID: {mealName.MealNameId} Name: {mealName.MealName} Type: {mealName.MealType}");
            }

            return sb.ToString();
        }

        private string HandleAddMenuNameRequest(string request)
        {
            var parts = request.Split('|');
            
            string jsonMealName = parts[1];
            MealNameDTO mealName = JsonConvert.DeserializeObject<MealNameDTO>(jsonMealName);  

            _mealNameService.AddMealName(mealName);

            return "Meal Added";
        }

        private string HandleGetMenuByIdRequest(string request)
        {
            var parts = request.Split('|');

            var mealNameId = Convert.ToInt32(parts[1]);

            var mealName = _mealNameService.GetMealName(mealNameId);

            return ($"ID: {mealName.MealNameId} Name: {mealName.MealName} Type: {mealName.MealType}");
        }

        private string HandleUpdateMenuNameRequest(string request)
        {
            var parts = request.Split('|');

            string jsonMealName = parts[1];
            MealNameDTO mealName = JsonConvert.DeserializeObject<MealNameDTO>(jsonMealName);

            _mealNameService.UpdateMealName(mealName);

            return "Meal Updated";
        }

        private string HandleDeleteMenuRequest(string request)
        {
            var parts = request.Split('|');

            var mealNameId = Convert.ToInt32(parts[1]);

            _mealNameService.DeleteMealName(mealNameId);

            return ($"Meal id {parts[1]} deleted sucessfully");
        }
    }

}
