namespace DataAcessLayer.Repository.Repository
{
    public class NotificationRepository : IRepository<Notification>
    {
        private readonly FoodRecommendationContext _context;

        public NotificationRepository(FoodRecommendationContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetAll()
        {
            return _context.Notifications.ToList();
        }

        public Notification GetById(int id)
        {
            return _context.Notifications.Find(id);
        }

        public void Insert(Notification entity)
        {
            _context.Notifications.Add(entity);
        }

        public void Update(Notification entity)
        {
            _context.Notifications.Update(entity);
        }

        public void Delete(int id)
        {
            var notification = _context.Notifications.Find(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
