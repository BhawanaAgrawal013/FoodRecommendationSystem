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

        public ICollection<Meal> Meals { get; set; }
    }
}