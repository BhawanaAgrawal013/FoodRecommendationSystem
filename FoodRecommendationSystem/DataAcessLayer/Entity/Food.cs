namespace DataAcessLayer.Entity;

public class Food
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Column(TypeName = "text")]
    public string Description { get; set; }

    [MaxLength(500)]
    public string Summary { get; set; }

    public double Price { get; set; }

    public bool IsAvailable { get; set; }

    [MaxLength(100)]
    public string LchfMenu { get; set; }

    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<MealPlan> MealPlans { get; set; }
    public SummaryRating SummaryRating { get; set; }
}