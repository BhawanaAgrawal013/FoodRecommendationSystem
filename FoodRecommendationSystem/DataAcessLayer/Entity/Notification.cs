namespace DataAcessLayer.Entity
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string NotificationMessage { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}