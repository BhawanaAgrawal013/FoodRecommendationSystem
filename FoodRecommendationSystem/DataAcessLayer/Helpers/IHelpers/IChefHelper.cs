namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IChefHelper
    {
        MealMenuDTO ChooseNextMealMenu(int mealMenuId);
        void CreateNextMealMenu(List<string> mealNames, string classification);
        List<MealDTO> SendDishesToReview(List<string> mealNames);
        List<MealMenuDTO> ViewEmployeeVotes(string classification, DateTime dateTime);
    }
}