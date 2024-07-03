using Serilog;

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
            try
            {
                DiscardedMenu discardedMenu = (DiscardedMenu)discardedMenuDTO;
                _discardedMenuRepository.Insert(discardedMenu);
                _discardedMenuRepository.Save();
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding discarded menu: {ex.Message}");
                throw new Exception("Error adding discarded menu", ex);
            }
        }

        public List<DiscardedMenuDTO> GetDiscardedMenuList()
        {
            try
            {
                var discardedMenus = _discardedMenuRepository.GetAll();
                return discardedMenus.Select(dm => (DiscardedMenuDTO)dm).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting discarded menu list: {ex.Message}");
                throw new Exception("Error getting discarded menu list", ex);
            }
        }

        public DiscardedMenuDTO GetDiscardedMenu(int id)
        {
            try
            {
                var discardedMenu = _discardedMenuRepository.GetById(id);
                if (discardedMenu == null)
                {
                    throw new Exception($"Discarded menu with id {id} not found");
                }
                return (DiscardedMenuDTO)discardedMenu;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting discarded menu with id {id}: {ex.Message}");
                throw new Exception($"Error getting discarded menu with id {id}", ex);
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
                Log.Error($"Error updating discarded menu: {ex.Message}");
                throw new Exception("Error updating discarded menu", ex);
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
                Log.Error($"Error deleting discarded menu with id {id}: {ex.Message}");
                throw new Exception($"Error deleting discarded menu with id {id}", ex);
            }
        }
    }
}
