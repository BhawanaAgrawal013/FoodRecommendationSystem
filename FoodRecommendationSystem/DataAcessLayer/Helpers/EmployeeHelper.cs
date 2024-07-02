namespace DataAcessLayer.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private IMealMenuService _mealMenuService;
        private readonly IProfileService _profileService;
        public EmployeeHelper(IMealMenuService mealMenuRepository,  IProfileService profileService) 
        { 
            _mealMenuService = mealMenuRepository;
            _profileService = profileService;
        }

        public List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification, string email)
        {
            try
            {
                var mealMenus = _mealMenuService.GetAllMealMenus().Where(x => x.CreationDate == dateTime && x.Classification == classification).ToList();
                return SortForProfile(email, mealMenus);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the mean menu options of {classification} for {email}");
            }
        }

        public MealMenuDTO GetNextDayMealMenu(DateTime dateTime, string classification)
        {
            try
            {
                var mealMenu = _mealMenuService.GetAllMealMenus().Where(x => x.CreationDate == dateTime && x.Classification == classification
                                                                  && x.WasPrepared).FirstOrDefault();
                return (MealMenuDTO)mealMenu;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the meal menu at {dateTime}", ex);
            }
        }

        public MealMenuDTO VoteForNextDayMeal(int mealMenuId, DateTime dateTime)
        {
            try
            {
                var mealMenu = _mealMenuService.GetAllMealMenus()
                    .FirstOrDefault(x => x.Id == mealMenuId && x.CreationDate == dateTime);

                if (mealMenu != null)
                {
                    mealMenu.NumberOfVotes += 1;

                    _mealMenuService.UpdateMealMenu(mealMenu);

                    return mealMenu;
                }
                else
                {
                    throw new Exception("Meal menu not found for the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot vote for the next day meal", ex);
            }
        }

        private List<MealMenuDTO> SortForProfile(string email, List<MealMenuDTO> mealMenuDTOs)
        {
            try
            {
                var profile = _profileService.GetAllProfiles().Where(x => x.User.Email == email).FirstOrDefault();

                var mealNames = mealMenuDTOs.Select(x => x.MealName).ToList();

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
                throw new Exception($"Error sorting the meals based on {email} profile");
            }
        }
    }
}
