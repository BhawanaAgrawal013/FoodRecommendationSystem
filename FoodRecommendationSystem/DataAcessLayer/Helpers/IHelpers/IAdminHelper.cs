namespace DataAcessLayer.Helpers.IHelpers
{
    public interface IAdminHelper
    {
        List<MealNameDTO> GetFullMenu();
        MealNameDTO AddMenuItem(MealNameDTO mealDTO);
        MealNameDTO UpdateMenuItem(MealNameDTO mealDTO);
        string DeleteMenuItem(int mealNameId);
    }
}
