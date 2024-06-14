namespace DataAcessLayer.Entity;

public class Food
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(200)")]
    public string Description { get; set; }

    public double Price { get; set; }

    public bool IsAvailable { get; set; }

    public bool IsInMainMenu { get; set; }

    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Meal> MealPlans { get; set; }
    public SummaryRating SummaryRating { get; set; }
}