namespace DataAcessLayer.Service.IService
{
    public interface IRecommendationEngineService
    {
        List<RecommendedMeal> GiveRecommendation(string classification, int numberOfMeals);
    }
}