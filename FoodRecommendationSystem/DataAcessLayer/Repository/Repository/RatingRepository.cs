namespace DataAcessLayer.Repository.Repository
{
    public class RatingRepository : IRepository<Rating>
    {
        private readonly FoodRecommendationContext _context;

        public RatingRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<Rating> GetAll()
        {
            return _context.Ratings.Include(x => x.User).Include(x => x.Food).ToList();
        }

        public Rating GetById(int id)
        {
            return _context.Ratings.Include(x => x.User).Include(x => x.Food).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(Rating entity)
        {
            _context.Ratings.Add(entity);
        }

        public void Update(Rating entity)
        {
            var existingEntity = _context.Ratings.Find(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Ratings.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            var rating = _context.Ratings.Find(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
