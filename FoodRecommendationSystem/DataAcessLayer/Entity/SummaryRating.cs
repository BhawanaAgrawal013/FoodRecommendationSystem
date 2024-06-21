namespace DataAcessLayer.Entity;

public class SummaryRating
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    public double SentimentScore { get; set; }

    public double AverageRating { get; set; }

    public double TotalAppearanceRating { get; set; }

    public double TotalQuantityRating { get; set; }

    public double TotalQualityRating { get; set; }

    public double TotalValueForMoneyRating {  get; set; }

    public int NumberOfPeople { get; set; }

    public string SentimentComment { get; set; }

    public Food Food { get; set; }
}