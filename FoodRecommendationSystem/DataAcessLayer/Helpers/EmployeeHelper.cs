namespace DataAcessLayer.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private IMealMenuService _mealMenuService;
        private readonly IFeedbackHelper _feedbackHelper;
        public EmployeeHelper(IMealMenuService mealMenuRepository, IFeedbackHelper feedbackHelper) 
        { 
            _mealMenuService = mealMenuRepository;
            _feedbackHelper = feedbackHelper;
        }

        public List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification)
        {
            try
            {
                var mealMenus = _mealMenuService.GetAllMealMenus().Where(x => x.CreationDate == dateTime && x.Classification == classification).ToList();
                return mealMenus.Select(mealMenu => (MealMenuDTO)mealMenu).ToList();
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

        public MealMenuDTO VoteForNextDayMeal(int mealNameId, DateTime dateTime)
        {
            try
            {
                var mealMenu = _mealMenuService.GetAllMealMenus()
                    .FirstOrDefault(x => x.MealName.MealNameId == mealNameId && x.CreationDate == dateTime);

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
    }
}
