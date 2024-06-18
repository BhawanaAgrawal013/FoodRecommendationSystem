namespace DataAcessLayer.Service.IService
{
    public interface INotificationService
    {
        void AddNotification(NotificationDTO notificationDTO);
        void DeleteNotification(int id);
        List<NotificationDTO> GetAllNotifications();
        NotificationDTO GetNotification(int id);
        void UpdateNotification(NotificationDTO notificationDTO);
    }
}