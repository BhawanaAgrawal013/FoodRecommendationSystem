namespace DataAcessLayer.Service.Service
{
    public class DiscardedMenuService : IDiscardedMenuService
    {
        private readonly IRepository<DiscardedMenu> _discardedMenuRepository;

        public DiscardedMenuService(IRepository<DiscardedMenu> discardedMenuRepository)
        {
            _discardedMenuRepository = discardedMenuRepository;
        }

        public void AddDiscardedMenu(DiscardedMenuDTO discardedMenuDTO)
        {
            DiscardedMenu discardedMenu = (DiscardedMenu)discardedMenuDTO;
            _discardedMenuRepository.Insert(discardedMenu);
            _discardedMenuRepository.Save();
        }

        public List<DiscardedMenuDTO> GetDiscardedMenuList()
        {
            var discardedMenus = _discardedMenuRepository.GetAll();
            return discardedMenus.Select(dm => (DiscardedMenuDTO)dm).ToList();
        }

        public DiscardedMenuDTO GetDiscardedMenu(int id)
        {
            try
            {
                var discardedMenu = _discardedMenuRepository.GetById(id);
                return (DiscardedMenuDTO)discardedMenu;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the food {id}", ex);
            }
        }

        public void UpdateDiscardedMenu(DiscardedMenuDTO discardedMenuDTO)
        {
            try
            {
                var discardedMenu = (DiscardedMenu)discardedMenuDTO;
                _discardedMenuRepository.Update(discardedMenu);
                _discardedMenuRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the food", ex);
            }
        }

        public void DeleteDiscardedMenu(int id)
        {
            try
            {
                _discardedMenuRepository.Delete(id);
                _discardedMenuRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the food {id}", ex);
            }
        }
    }
}
