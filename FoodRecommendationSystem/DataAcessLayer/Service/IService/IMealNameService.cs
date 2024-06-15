namespace DataAcessLayer.Service.IService
{
    public interface IMealNameService
    {
        string AddMealName(MealNameDTO mealNameDTO);

        List<MealNameDTO> GetAllMeals();
    }
}
