namespace DataAcessLayer.Repository.IRepository
{
    public interface IMealNameRepository
    {
        string AddMealName (MealNameDTO mealNameDTO);

        List<MealNameDTO> GetAllMeals();

        void Save();
    }
}
