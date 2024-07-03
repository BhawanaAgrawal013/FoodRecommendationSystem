using Serilog;

namespace DataAcessLayer.Helpers;

public class NotificationHelper : INotificationHelper
{
    private readonly INotificationService _notificationService;
    private readonly IUserNotificationService _userNotificationService;
    private readonly IUserService _user;

    public NotificationHelper(IUserNotificationService userNotificationService, INotificationService notificationService,
        IUserService repository)
    {
        _notificationService = notificationService;
        _user = repository;
        _userNotificationService = userNotificationService;
    }

    public void AddNotification(string message)
    {
        try
        {
            var notificationDTO = new NotificationDTO
            {
                NotificationMessage = message,
                DateTime = DateTime.Now
            };

            _notificationService.AddNotification(notificationDTO);

            var notification = _notificationService.GetAllNotifications().LastOrDefault();
            if (notification == null)
            {
                throw new Exception("Notification was not added successfully.");
            }

            var users = _user.GetAllUsers().Where(x => x.Role.RoleName == UserRole.Employee.ToString());

            foreach (var user in users)
            {
                var userNotificationDTO = new UserNotificationDTO
                {
                    Notification = notification,
                    User = user,
                    IsRead = false
                };

                _userNotificationService.AddUserNotification(userNotificationDTO);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error adding notification: {ex.Message}");
            throw new Exception("Error adding notification", ex);
        }
    }

    public List<UserNotificationDTO> GetUnreadNotification(string email)
    {
        try
        {
            var user = _user.GetAllUsers().FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var userNotifications = _userNotificationService.GetAllUserNotifications()
                .Where(x => x.User.Email == email && !x.IsRead && x.Notification.DateTime.Date >= DateTime.Now.Date)
                .ToList();

            if (userNotifications.Any())
            {
                _userNotificationService.MarkNotificationAsRead(user.Id);
            }

            return userNotifications;
        }
        catch (Exception ex)
        {
            Log.Error($"Error getting unread notifications for user '{email}': {ex.Message}");
            throw new Exception($"Error getting unread notifications for user '{email}'", ex);
        }
    }
}