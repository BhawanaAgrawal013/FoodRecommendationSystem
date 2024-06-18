namespace DataAcessLayer.Repository.Repository
{
    public class MealMenuRepository : IRepository<MealMenu>
    {
        private readonly FoodRecommendationContext _context;

        public MealMenuRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<MealMenu> GetAll()
        {
            return _context.MealMenus.Include(x => x.MealName).ToList();
        }

        public MealMenu GetById(int id)
        {
            return _context.MealMenus.Include(x => x.MealName).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(MealMenu entity)
        {
            _context.MealMenus.Add(entity);
        }

        public void Update(MealMenu entity)
        {
            _context.MealMenus.Update(entity);
        }

        public void Delete(int id)
        {
            var mealMenu = _context.MealMenus.Find(id);
            if (mealMenu != null)
            {
                _context.MealMenus.Remove(mealMenu);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
