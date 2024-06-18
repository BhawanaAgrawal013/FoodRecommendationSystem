namespace DataAcessLayer.Service.IService
{
    public interface IMealMenuService
    {
        void AddMealMenu(MealMenuDTO mealMenuDTO);
        void DeleteMealMenu(int id);
        List<MealMenuDTO> GetAllMealMenus();
        MealMenuDTO GetMealMenu(int id);
        void UpdateMealMenu(MealMenuDTO mealMenuDTO);
    }
}