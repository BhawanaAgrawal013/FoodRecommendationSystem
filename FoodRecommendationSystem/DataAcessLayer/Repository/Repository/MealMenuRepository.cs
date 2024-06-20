using DataAcessLayer.Entity;

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
            var existingMealName = _context.MealNames.Local.FirstOrDefault(mn => mn.Id == entity.MealNameId);

            if (existingMealName != null)
            {
                entity.MealName = existingMealName;
            }
            else
            {
                _context.MealNames.Attach(entity.MealName);
            }

            _context.MealMenus.Add(entity);
        }

        public void Update(MealMenu entity)
        {
            try
            {
                var existingEntity = _context.MealMenus.Find(entity.Id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }
                
                _context.MealMenus.Attach(entity);

                _context.Entry(entity).State = EntityState.Modified;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating meal menu", ex);
            }
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
