namespace DataAcessLayer.Service.IService
{
    public interface IMealService
    {
        int AddMeal(MealDTO mealDTO);
        void DeleteMeal(int id);
        List<MealDTO> GetAllMeals();
        MealDTO GetMealById(int id);
        void UpdateMeal(MealDTO mealDTO);
    }
}