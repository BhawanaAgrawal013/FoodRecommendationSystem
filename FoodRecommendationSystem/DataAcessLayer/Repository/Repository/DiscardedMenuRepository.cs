namespace DataAcessLayer.Repository.Repository
{
    public class DiscardedMenuRepository : IRepository<DiscardedMenu>
    {
        public readonly FoodRecommendationContext _context;

        public DiscardedMenuRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<DiscardedMenu> GetAll()
        {
            return _context.DiscardedMenus.Include(x => x.MealName).ToList();
        }

        public DiscardedMenu GetById(int id)
        {
            return _context.DiscardedMenus.Include(x => x.MealName).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(DiscardedMenu entity)
        {
            _context.DiscardedMenus.Add(entity);
        }

        public void Update(DiscardedMenu entity)
        {
            _context.DiscardedMenus.Update(entity);
        }

        public void Delete(int id)
        {
            var discardedMenu = _context.DiscardedMenus.Find(id);
            if (discardedMenu != null)
            {
                _context.DiscardedMenus.Remove(discardedMenu);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
