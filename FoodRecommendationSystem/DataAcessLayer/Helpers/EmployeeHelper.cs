namespace DataAcessLayer.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private IRepository<MealMenu> _mealMenuRepository;
        private readonly IFeedbackHelper _feedbackHelper;
        public EmployeeHelper(IRepository<MealMenu> mealMenuRepository, IFeedbackHelper feedbackHelper) 
        { 
            _mealMenuRepository = mealMenuRepository;
            _feedbackHelper = feedbackHelper;
        }

        public List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification)
        {
            try
            {
                var mealMenus = _mealMenuRepository.GetAll().Where(x => x.CreationDate == dateTime && x.Classification == classification).ToList();
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
                var mealMenu = _mealMenuRepository.GetAll().Where(x => x.CreationDate == dateTime && x.Classification == classification
                                                                  && x.WasPrepared).FirstOrDefault();
                return (MealMenuDTO)mealMenu;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the meal menu at {dateTime}", ex);
            }
        }

        public MealMenuDTO VoteForNextDayMeal(int mealMenuId)
        {
            try
            {
                var mealMenu = _mealMenuRepository.GetById(mealMenuId);
                mealMenu.NumberOfVotes += 1;

                _mealMenuRepository.Update(mealMenu);

                return mealMenu;
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
