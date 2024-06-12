namespace DataAcessLayer.Entity;

public class SummaryRating
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    [MaxLength(500)]
    public string ReviewSummary { get; set; }

    public double OverallRating { get; set; }

    public Food Food { get; set; }
}