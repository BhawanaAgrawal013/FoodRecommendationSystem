namespace DataAcessLayer.Helpers.IHelpers
{
    public interface INotificationHelper
    {
        void AddNotification(string message);
        List<UserNotificationDTO> GetUnreadNotification(string email);
    }
}