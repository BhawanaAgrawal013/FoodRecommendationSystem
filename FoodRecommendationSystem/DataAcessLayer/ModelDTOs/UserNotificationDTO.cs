namespace DataAcessLayer.ModelDTOs
{
    public class UserNotificationDTO
    {
        public int Id { get; set; }

        public bool IsRead { get; set; }

        public UserDTO User { get; set; }
        public NotificationDTO Notification { get; set; }

        public static implicit operator UserNotificationDTO(UserNotification userNotification)
        {
            if (userNotification == null) return null;

            return new UserNotificationDTO()
            {
                Id = userNotification.Id,
                IsRead = userNotification.IsRead,
                Notification = (NotificationDTO)userNotification.Notification,
                User = (UserDTO)userNotification.User
            };
        }

        public static implicit operator UserNotification(UserNotificationDTO userNotificationDTO)
        {
            if (userNotificationDTO == null) return null;

            return new UserNotification()
            {
                IsRead = userNotificationDTO.IsRead,
                UserId = userNotificationDTO.User.Id,
                NotificationId = userNotificationDTO.Notification.Id
            };
        }
    }
}
