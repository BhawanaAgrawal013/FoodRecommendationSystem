using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class MealMenuService : IMealMenuService
    {
        private readonly IRepository<MealMenu> _mealMenuRepository;

        public MealMenuService(IRepository<MealMenu> mealMenuRepository)
        {
            _mealMenuRepository = mealMenuRepository;
        }

        public void AddMealMenu(MealMenuDTO mealMenuDTO)
        {
            try
            {
                MealMenu mealMenu = (MealMenu)mealMenuDTO;
                _mealMenuRepository.Insert(mealMenu);
                _mealMenuRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting meal menu", ex);
            }
        }

        public List<MealMenuDTO> GetAllMealMenus()
        {
            try
            {
                var mealMenus = _mealMenuRepository.GetAll();
                return mealMenus.Select(mealMenu => (MealMenuDTO)mealMenu).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all meal menus", ex);
            }
        }

        public MealMenuDTO GetMealMenu(int id)
        {
            try
            {
                var mealMenu = _mealMenuRepository.GetById(id);
                return (MealMenuDTO)mealMenu;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the meal menu {id}", ex);
            }
        }

        public void UpdateMealMenu(MealMenuDTO mealMenuDTO)
        {
            try
            {
                var mealMenu = (MealMenu)mealMenuDTO;
                _mealMenuRepository.Update(mealMenu);
                _mealMenuRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the meal menu", ex);
            }
        }

        public void DeleteMealMenu(int id)
        {
            try
            {
                _mealMenuRepository.Delete(id);
                _mealMenuRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the meal menu {id}", ex);
            }
        }
    }

}
