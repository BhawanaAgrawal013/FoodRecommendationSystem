namespace DataAcessLayer.Entity
{
    public class UserNotification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Notification")]
        public int NotificationId { get; set; }

        public bool IsRead { get; set; }

        public User User { get; set; }
        public Notification Notification { get; set; }
    }
}
