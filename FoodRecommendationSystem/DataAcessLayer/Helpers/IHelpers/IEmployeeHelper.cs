namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IEmployeeHelper
    {
        List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification);
        MealMenuDTO GetNextDayMealMenu(DateTime dateTime, string classification);
        MealMenuDTO VoteForNextDayMeal(int mealMenuId, DateTime dateTime);
        void GiveFeedback(RatingDTO ratingDTO, ReviewDTO reviewDTO);
    }
}
