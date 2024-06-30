namespace DataAcessLayer.Repository.Repository
{
    public class MealNameRepository : IRepository<MealName>
    {
        private readonly FoodRecommendationContext _context;
        public MealNameRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<MealName> GetAll()
        {
            return _context.MealNames.Where(x => !x.IsDeleted).ToList();
        }

        public MealName GetById(int id)
        {
            return _context.MealNames.Find(id);
        }

        public void Insert(MealName entity)
        {
            _context.MealNames.Add(entity);
        }

        public void Update(MealName entity)
        {
            var existingEntity = _context.MealNames.Find(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.MealNames.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var mealName = _context.MealNames.Find(id);
            if (mealName != null)
            {
                _context.MealNames.Remove(mealName);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
