using Serilog;

namespace DataAcessLayer.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private IMealMenuService _mealMenuService;
        private readonly IProfileService _profileService;
        public EmployeeHelper(IMealMenuService mealMenuRepository, IProfileService profileService)
        {
            _mealMenuService = mealMenuRepository;
            _profileService = profileService;
        }

        public List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification, string email)
        {
            try
            {
                var mealMenus = _mealMenuService.GetAllMealMenus()
                    .Where(x => x.CreationDate == dateTime && x.Classification == classification)
                    .ToList();

                if (!mealMenus.Any())
                {
                    throw new Exception($"No meal menus found for classification '{classification}' on date '{dateTime.ToShortDateString()}'.");
                }

                return SortForProfile(email, mealMenus);
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the meal menu options for classification '{classification}' and email '{email}': {ex.Message}");
                throw new Exception($"Error getting the meal menu options for classification '{classification}' and email '{email}': {ex.Message}");
            }
        }

        public MealMenuDTO GetNextDayMealMenu(DateTime dateTime, string classification)
        {
            try
            {
                var mealMenu = _mealMenuService.GetAllMealMenus()
                    .Where(x => x.CreationDate == dateTime && x.Classification == classification && x.WasPrepared)
                    .FirstOrDefault();

                if (mealMenu == null)
                {
                    throw new Exception($"No prepared meal menu found for classification '{classification}' on date '{dateTime.ToShortDateString()}'.");
                }

                return mealMenu;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting the meal menu for date '{dateTime.ToShortDateString()}' and classification '{classification}': {ex.Message}");
                throw new Exception($"Error getting the meal menu for date '{dateTime.ToShortDateString()}' and classification '{classification}': {ex.Message}");
            }
        }

        public MealMenuDTO VoteForNextDayMeal(int mealMenuId, DateTime dateTime)
        {
            try
            {
                var mealMenu = _mealMenuService.GetAllMealMenus()
                    .FirstOrDefault(x => x.Id == mealMenuId && x.CreationDate == dateTime);

                if (mealMenu == null)
                {
                    throw new Exception($"Meal menu not found for ID '{mealMenuId}' on date '{dateTime.ToShortDateString()}'.");
                }

                mealMenu.NumberOfVotes += 1;
                _mealMenuService.UpdateMealMenu(mealMenu);

                return mealMenu;
            }
            catch (Exception ex)
            {
                Log.Error($"Error voting for the next day meal with ID '{mealMenuId}' on date '{dateTime.ToShortDateString()}': {ex.Message}");
                throw new Exception($"Error voting for the next day meal with ID '{mealMenuId}' on date '{dateTime.ToShortDateString()}': {ex.Message}");
            }
        }

        private List<MealMenuDTO> SortForProfile(string email, List<MealMenuDTO> mealMenuDTOs)
        {
            try
            {
                var profile = _profileService.GetAllProfiles()
                    .FirstOrDefault(x => x.User.Email == email);

                if(profile == null)
                {
                    return mealMenuDTOs;
                }

                foreach (var meal in mealMenuDTOs)
                {
                    if (profile.DietType == meal.MealName.DietType)
                    {
                        meal.Priority += 4;
                    }
                    if (profile.CuisinePreference == meal.MealName.CuisinePreference)
                    {
                        meal.Priority += 3;
                    }
                    if (profile.SpiceLevel == meal.MealName.SpiceLevel)
                    {
                        meal.Priority += 2;
                    }
                    if (profile.IsSweet == meal.MealName.IsSweet)
                    {
                        meal.Priority += 1;
                    }
                }

                return mealMenuDTOs.OrderByDescending(x => x.Priority).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error sorting the meals based on profile for email '{email}': {ex.Message}");
                throw new Exception($"Error sorting the meals based on profile for email '{email}': {ex.Message}");
            }
        }
    }
}
