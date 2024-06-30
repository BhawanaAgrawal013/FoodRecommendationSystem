namespace DataAcessLayer.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private IMealMenuService _mealMenuService;
        private readonly IFeedbackHelper _feedbackHelper;
        private readonly IProfileService _profileService;
        public EmployeeHelper(IMealMenuService mealMenuRepository, IFeedbackHelper feedbackHelper,
                        IProfileService profileService) 
        { 
            _mealMenuService = mealMenuRepository;
            _feedbackHelper = feedbackHelper;
            _profileService = profileService;
        }

        public List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification, string email)
        {
            try
            {
                var mealMenus = _mealMenuService.GetAllMealMenus().Where(x => x.CreationDate == dateTime && x.Classification == classification).ToList();
                var mealDtos = mealMenus.Select(mealMenu => (MealMenuDTO)mealMenu).ToList();
                return SortForProfile(email, mealDtos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the meal menu at {dateTime}", ex);
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

        public void GiveFeedback(RatingDTO ratingDTO, ReviewDTO reviewDTO)
        {
            _feedbackHelper.AddFeedback(reviewDTO, ratingDTO);
        }

        private List<MealMenuDTO> SortForProfile(string email, List<MealMenuDTO> mealMenuDTOs)
        {
            var profile = _profileService.GetAllProfiles().Where(x => x.User.Email == email).FirstOrDefault();

            var mealNames = mealMenuDTOs.Select(x => x.MealName).ToList();

            foreach(var meal in mealMenuDTOs)
            {
                if(profile.DietType == meal.MealName.DietType)
                {
                    meal.Priority += 4;
                }
                if(profile.CuisinePreference == meal.MealName.CuisinePreference)
                {
                    meal.Priority += 3;
                }
                if(profile.SpiceLevel == meal.MealName.SpiceLevel)
                {
                    meal.Priority += 2;
                }
                if(profile.IsSweet == meal.MealName.IsSweet)
                {
                    meal.Priority += 1;
                }
            }

            return mealMenuDTOs.OrderByDescending(x => x.Priority).ToList();
        }
    }
}
