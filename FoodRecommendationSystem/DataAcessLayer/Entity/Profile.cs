namespace DataAcessLayer.Entity
{
    public class Profile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string DietType { get; set; }

        public string SpiceLevel { get; set; }

        public string CuisinePreference { get; set; }

        public bool IsSweet { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
