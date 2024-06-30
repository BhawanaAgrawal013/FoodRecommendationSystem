namespace DataAcessLayer.Repository.Repository
{
    public class DiscardedMenuFeedbackRepository : IRepository<DiscardedMenuFeedback>
    {
        public readonly FoodRecommendationContext _context;

        public DiscardedMenuFeedbackRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<DiscardedMenuFeedback> GetAll()
        {
           return _context.DiscardedMenuFeedbacks.Include(x => x.DiscardedMenu).ToList();
        }

        public DiscardedMenuFeedback GetById(int id)
        {
            return _context.DiscardedMenuFeedbacks.Include(x => x.DiscardedMenu).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(DiscardedMenuFeedback entity)
        {
            _context.DiscardedMenuFeedbacks.Add(entity);
        }

        public void Update(DiscardedMenuFeedback entity)
        {
            _context.DiscardedMenuFeedbacks.Update(entity);
        }

        public void Delete(int id)
        {
            var discardedMenu = _context.DiscardedMenuFeedbacks.Find(id);
            if (discardedMenu != null)
            {
                _context.DiscardedMenuFeedbacks.Remove(discardedMenu);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
