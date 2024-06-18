namespace DataAcessLayer.Repository.Repository
{
    public class SummaryRatingRepository : IRepository<SummaryRating>
    {
        private readonly FoodRecommendationContext _context;

        public SummaryRatingRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<SummaryRating> GetAll()
        {
            return _context.SummaryRatings.Include(x => x.Food).ToList();
        }

        public SummaryRating GetById(int id)
        {
            return _context.SummaryRatings.Include(x => x.Food).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(SummaryRating entity)
        {
            _context.SummaryRatings.Add(entity);
        }

        public void Update(SummaryRating entity)
        {
            _context.SummaryRatings.Update(entity);
        }

        public void Delete(int id)
        {
            var summaryRating = _context.SummaryRatings.Find(id);
            if (summaryRating != null)
            {
                _context.SummaryRatings.Remove(summaryRating);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
