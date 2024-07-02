namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IEmployeeHelper
    {
        List<MealMenuDTO> GetMealMenuOption(DateTime dateTime, string classification, string email);
        MealMenuDTO GetNextDayMealMenu(DateTime dateTime, string classification);
        MealMenuDTO VoteForNextDayMeal(int mealMenuId, DateTime dateTime);
    }
}
