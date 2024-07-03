using DataAcessLayer.Service.IService;
using Serilog;

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
                Log.Error($"Error adding notification: {ex.Message}");
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
                Log.Error($"Error getting all notifications: {ex.Message}");
                throw new Exception("Error getting all notifications", ex);
            }
        }

        public NotificationDTO GetNotification(int id)
        {
            try
            {
                var notification = _notificationRepository.GetById(id);
                if (notification == null)
                {
                    throw new Exception($"Notification with id {id} not found");
                }
                return (NotificationDTO)notification;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting notification with id {id}: {ex.Message}");
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
                Log.Error($"Error updating notification: {ex.Message}");
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
                Log.Error($"Error deleting notification with id {id}: {ex.Message}");
                throw new Exception($"Error deleting the notification {id}", ex);
            }
        }

    }

}
