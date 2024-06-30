namespace DataAcessLayer.Service.IService
{
    public interface IDiscardedMenuService
    {
        void AddDiscardedMenu(DiscardedMenuDTO discardedMenuDTO);
        void DeleteDiscardedMenu(int id);
        DiscardedMenuDTO GetDiscardedMenu(int id);
        List<DiscardedMenuDTO> GetDiscardedMenuList();
        void UpdateDiscardedMenu(DiscardedMenuDTO discardedMenuDTO);
    }
}