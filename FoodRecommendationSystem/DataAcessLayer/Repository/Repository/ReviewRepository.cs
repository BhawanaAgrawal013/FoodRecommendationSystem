namespace DataAcessLayer.Repository.Repository
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly FoodRecommendationContext _context;

        public ReviewRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<Review> GetAll()
        {
            return _context.Reviews.Include(x => x.User).Include(x => x.Food).ToList();
        }

        public Review GetById(int id)
        {
            return _context.Reviews.Include(x => x.User).Include(x => x.Food).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(Review entity)
        {
            _context.Reviews.Add(entity);
        }

        public void Update(Review entity)
        {
            try
            {
                var existingEntity = _context.Reviews.Find(entity.Id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }

                _context.Reviews.Attach(entity);

                _context.Entry(entity).State = EntityState.Modified;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating review", ex);
            }
        }

        public void Delete(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
