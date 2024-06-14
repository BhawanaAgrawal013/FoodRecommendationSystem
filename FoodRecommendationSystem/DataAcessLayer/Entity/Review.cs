namespace DataAcessLayer.Entity;

public class Review
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    [Column(TypeName = "varchar(100)")]
    [Required]
    public string ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public double OverallRating { get; set; }

    public int QuantityRating { get; set; }

    public int QualityRating { get; set; }
    
    public int AppearanceRating { get; set; }

    public int ValueForMoneyRating { get; set; }

    public User User { get; set; }
    public Food Food { get; set; }
}