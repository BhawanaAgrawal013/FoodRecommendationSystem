namespace DataAcessLayer.ModelDTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        
        public string NotificationMessage { get; set; }

        public DateTime DateTime { get; set; }

        public static implicit operator NotificationDTO(Notification notification)
        {
            if (notification == null) return null;

            return new NotificationDTO()
            {
                Id = notification.Id,
                NotificationMessage = notification.NotificationMessage,
                DateTime = notification.DateTime
            };
        }

        public static implicit operator Notification(NotificationDTO notificationDTO)
        {
            if (notificationDTO == null) return null;

            return new Notification()
            {
                Id = notificationDTO.Id,
                NotificationMessage = notificationDTO.NotificationMessage,
                DateTime = notificationDTO.DateTime
            };
        }
    }
}
