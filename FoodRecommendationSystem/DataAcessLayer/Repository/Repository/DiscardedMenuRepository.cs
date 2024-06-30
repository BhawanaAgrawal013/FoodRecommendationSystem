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
            var existingEntity = _context.DiscardedMenus.Find(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.DiscardedMenus.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();
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
