using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.ModelDTOs;
using Newtonsoft.Json;
using Serilog;
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
                { "MENU_GET", GetMenuRequest },
                { "MENU_ADD", AddMenuNameRquest },
                { "MENU_UPDATE", UpdateMenuNameRequest },
                { "MENU_DELETE", DeleteMenuRequest },
                { "MENU_CLASSIFIED", GetMenuClassifiedRequest}
            };

            foreach (var handlerKey in requestHandlers.Keys)
            {
                if (request.StartsWith(handlerKey))
                {
                    var handler = requestHandlers[handlerKey];
                    return handler(request);
                }
            }

            return String.Empty;
        }

        private string GetMenuRequest(string request)
        {
            try
            {
                Log.Information("Getting full menu from database");
                var mealNames = _adminHelper.GetFullMenu();

                StringBuilder sb = new StringBuilder();

                foreach (var mealName in mealNames)
                {
                    sb.AppendLine($"ID: {mealName.MealNameId} Name: {mealName.MealName} Type: {mealName.MealType}");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting full menu {ex.Message}");
                throw new Exception($"Error getting full menu {ex.Message}");
            }
        }

        private string GetMenuClassifiedRequest(string request)
        {
            try
            {
                var parts = request.Split('|');
                var classification = parts[1];

                var mealNames = _adminHelper.GetFullMenu().Where(x => x.MealType == classification);
                StringBuilder sb = new StringBuilder();

                foreach (var mealName in mealNames)
                {
                    sb.AppendLine($"ID: {mealName.MealNameId} Name: {mealName.MealName} Type: {mealName.MealType}");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the menu by classification {ex.Message}");
                throw new Exception($"Error getting the menu by classification {ex.Message}");
            }
        }

        private string AddMenuNameRquest(string request)
        {
            try
            {
                var parts = request.Split('|');

                string jsonMealName = parts[1];
                MealNameDTO mealName = JsonConvert.DeserializeObject<MealNameDTO>(jsonMealName);

                Log.Information($"Added the meal {mealName}");
                _adminHelper.AddMenuItem(mealName);

                return "Meal Added";
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding the menu item {ex.Message}");
                throw new Exception($"Error adding the menu item {ex.Message}");
            }
        }

        private string UpdateMenuNameRequest(string request)
        {
            try
            {
                var parts = request.Split('|');

                string jsonMealName = parts[1];
                MealNameDTO mealName = JsonConvert.DeserializeObject<MealNameDTO>(jsonMealName);

                Log.Information($"Updating the meal item {mealName}");

                _adminHelper.UpdateMenuItem(mealName);

                return "Meal Updated";
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating the menu item {ex.Message}");
                throw new Exception($"Error updating the menu item {ex.Message}");
            }
        }

        private string DeleteMenuRequest(string request)
        {
            try
            {
                var parts = request.Split('|');

                var mealNameId = Convert.ToInt32(parts[1]);

                return _adminHelper.DeleteMenuItem(mealNameId);
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting the meal {ex.Message}");
                throw new Exception($"Error deleting the meal {ex.Message}");
            }
        }
    }
}
