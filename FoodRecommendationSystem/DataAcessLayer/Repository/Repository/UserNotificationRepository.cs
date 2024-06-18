namespace DataAcessLayer.Repository.Repository
{
    public class UserNotificationRepository : IRepository<UserNotification>
    {
        private readonly FoodRecommendationContext _context;

        public UserNotificationRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<UserNotification> GetAll()
        {
            return _context.UserNotifications.Include(x => x.User).Include(x => x.Notification).ToList();
        }

        public UserNotification GetById(int id)
        {
            return _context.UserNotifications.Include(x => x.User).Include(x => x.Notification).SingleOrDefault(x => x.Id == id);
        }

        public void Insert(UserNotification entity)
        {
            _context.UserNotifications.Add(entity);
        }

        public void Update(UserNotification entity)
        {
            _context.UserNotifications.Update(entity);
        }

        public void Delete(int id)
        {
            var userNotification = _context.UserNotifications.Find(id);
            if (userNotification != null)
            {
                _context.UserNotifications.Remove(userNotification);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
