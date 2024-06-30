namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IRecommendationHelper
    {
        RecommendedMeal AddDiscardedMeal(List<RecommendedMeal> recommendedMeals);
        List<RecommendedMeal> GetDiscardedMeals();
        DiscardedMenuDTO GetDiscardedMenu();
        void UpdateDiscardMeal(int discardId, bool isDiscarded);
    }
}