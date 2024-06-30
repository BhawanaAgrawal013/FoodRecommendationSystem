namespace DataAcessLayer.Repository.Repository
{
    public class ProfileRepository : IRepository<Profile>
    {
        private readonly FoodRecommendationContext _context;

        public ProfileRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<Profile> GetAll()
        {
            return _context.Profiles.Include(x => x.User).ToList();
        }

        public Profile GetById(int id)
        {
            return _context.Profiles.Include(x => x.User).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(Profile entity)
        {
            _context.Profiles.Add(entity);
        }

        public void Update(Profile entity)
        {
            var existingEntity = _context.Profiles.Find(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Profiles.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var profile = _context.Profiles.Find(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
