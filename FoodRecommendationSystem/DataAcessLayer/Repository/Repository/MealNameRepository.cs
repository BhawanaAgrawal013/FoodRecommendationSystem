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
            return _context.MealNames.ToList();
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
            _context.MealNames.Update(entity);
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
