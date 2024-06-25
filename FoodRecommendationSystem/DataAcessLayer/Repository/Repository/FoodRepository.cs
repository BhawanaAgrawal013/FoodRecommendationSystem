namespace DataAcessLayer.Repository.Repository
{
    public class FoodRepository : IRepository<Food>
    {
        private readonly FoodRecommendationContext _context;

        public FoodRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<Food> GetAll()
        {
            return _context.Foods.Where(x => !x.IsDeleted).ToList();
        }

        public Food GetById(int id)
        {
            return _context.Foods.Find(id);
        }

        public void Insert(Food entity)
        {
            _context.Foods.Add(entity);
        }

        public void Update(Food entity)
        {
            _context.Foods.Update(entity);
        }

        public void Delete(int id)
        {
            var food = _context.Foods.Find(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
