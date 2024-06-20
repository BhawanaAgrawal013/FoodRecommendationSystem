namespace DataAcessLayer.Service.IService
{
    public interface IMealNameService
    {
        void AddMealName(MealNameDTO mealNameDTO);

        List<MealNameDTO> GetAllMeals();

        MealNameDTO GetMealName(int id);

        void UpdateMealName(MealNameDTO mealNameDTO);

        void DeleteMealName(int id);
    }
}
