namespace DataAcessLayer.Entity
{
    public class MealName
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string MealType { get; set; }

        public string DietType { get; set; }

        public string SpiceLevel { get; set; }

        public string CuisinePreference { get; set; }

        public bool IsSweet { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Meal> Meals { get; set; }

        public DiscardedMenu DiscardedMenu { get; set; }
    }
}