namespace DataAcessLayer.Service.IService
{
    public interface IUserNotificationService
    {
        void AddUserNotification(UserNotificationDTO userNotificationDTO);
        void DeleteUserNotification(int id);
        List<UserNotificationDTO> GetAllUserNotifications();
        UserNotificationDTO GetUserNotification(int id);
        void UpdateUserNotification(UserNotificationDTO userNotificationDTO);
        List<UserNotificationDTO> GetUsersUnreadNotification(int userId);
        void MarkNotificationAsRead(int userId);
    }
}