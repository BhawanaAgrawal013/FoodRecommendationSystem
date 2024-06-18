using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notificationRepository;

        public NotificationService(IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void AddNotification(NotificationDTO notificationDTO)
        {
            try
            {
                Notification notification = (Notification)notificationDTO;
                _notificationRepository.Insert(notification);
                _notificationRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting notification", ex);
            }
        }

        public List<NotificationDTO> GetAllNotifications()
        {
            try
            {
                var notifications = _notificationRepository.GetAll();
                return notifications.Select(notification => (NotificationDTO)notification).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all notifications", ex);
            }
        }

        public NotificationDTO GetNotification(int id)
        {
            try
            {
                var notification = _notificationRepository.GetById(id);
                return (NotificationDTO)notification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the notification {id}", ex);
            }
        }

        public void UpdateNotification(NotificationDTO notificationDTO)
        {
            try
            {
                var notification = (Notification)notificationDTO;
                _notificationRepository.Update(notification);
                _notificationRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the notification", ex);
            }
        }

        public void DeleteNotification(int id)
        {
            try
            {
                _notificationRepository.Delete(id);
                _notificationRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the notification {id}", ex);
            }
        }
    }

}
