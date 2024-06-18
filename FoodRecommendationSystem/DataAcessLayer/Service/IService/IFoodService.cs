namespace DataAcessLayer.Service.IService
{
    public interface IFoodService
    {
        int AddFood(FoodDTO foodDTO);
        void DeleteFood(int id);
        List<FoodDTO> GetAllFoods();
        FoodDTO GetFood(int id);
        void UpdateFood(FoodDTO foodDTO);
    }
}