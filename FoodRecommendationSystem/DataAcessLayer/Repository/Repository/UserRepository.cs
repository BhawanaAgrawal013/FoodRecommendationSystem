namespace DataAcessLayer.Repository.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly FoodRecommendationContext _context;

        public UserRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(x => x.Role).ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Include(x => x.Role).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(User entity)
        {
            _context.Users.Add(entity);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
