namespace DataAcessLayer.Entity;

public class Review
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    [Column(TypeName = "text")]
    public string ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public double OverallRating { get; set; }

    [ForeignKey("Quantity")]
    public int QuantityId { get; set; }

    [ForeignKey("Quality")]
    public int QualityId { get; set; }

    [ForeignKey("Appearance")]
    public int AppearanceId { get; set; }

    public User User { get; set; }
    public Food Food { get; set; }
    public Quantity Quantity { get; set; }
    public Quality Quality { get; set; }
    public Appearance Appearance { get; set; }
}