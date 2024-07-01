using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using System.Text;

namespace Server.RequestHandlers
{
    public class MenuRequestHandler : IRequestHandler<MenuRequestHandler>
    {
        private readonly IAdminHelper _adminHelper;  
        public MenuRequestHandler(IAdminHelper adminHelper)
        {
            _adminHelper = adminHelper;  
        }

        public string HandleRequest(string request)
        {
            var requestHandlers = new Dictionary<string, Func<string, string>>
            {
                { "MENU_GET", HandleGetMenuRequest },
                { "MENU_ADD", HandleAddMenuNameRequest },
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
            var mealNames = _adminHelper.GetFullMenu();
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

            _adminHelper.AddMenuItem(mealName);

            return "Meal Added";
        }

        private string HandleUpdateMenuNameRequest(string request)
        {
            var parts = request.Split('|');

            string jsonMealName = parts[1];
            MealNameDTO mealName = JsonConvert.DeserializeObject<MealNameDTO>(jsonMealName);

            _adminHelper.UpdateMenuItem(mealName);

            return "Meal Updated";
        }

        private string HandleDeleteMenuRequest(string request)
        {
            var parts = request.Split('|');

            var mealNameId = Convert.ToInt32(parts[1]);

            _adminHelper.DeleteMenuItem(mealNameId);

            return ($"Meal id {parts[1]} deleted sucessfully");
        }
    }

}
