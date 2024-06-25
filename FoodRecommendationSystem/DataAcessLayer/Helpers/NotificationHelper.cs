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
        NotificationDTO notificationDTO = new NotificationDTO();
        notificationDTO.NotificationMessage = message;
        notificationDTO.DateTime = DateTime.Now;

        _notificationService.AddNotification(notificationDTO);
        var notification = _notificationService.GetAllNotifications().LastOrDefault();

        var users = _user.GetAllUsers().Where(x => x.Role.RoleName == "Employee");

        foreach (var user in users)
        {
            UserNotificationDTO userNotificationDTO = new UserNotificationDTO
            {
                Notification = notification,
                User = user,
                IsRead = false
            };

            _userNotificationService.AddUserNotification(userNotificationDTO);
        }
    }

    public List<UserNotificationDTO> GetUnreadNotification(string email)
    {
        var userNotifications = _userNotificationService.GetAllUserNotifications().Where(x => x.User.Email == email && !x.IsRead);

        int userId = _user.GetAllUsers().Where(x => x.Email == email).Select(x => x.Id).FirstOrDefault();

        _userNotificationService.MarkNotificationAsRead(userId);
        
        return userNotifications.ToList();
    }
}