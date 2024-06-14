namespace DataAcessLayer.Entity;

public class SummaryRating
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    [Required]
    public string ReviewSummary { get; set; }

    public double AverageRating { get; set; }

    public double TotalAppearanceRating { get; set; }

    public double TotalQuantityRating { get; set; }

    public double TotalQualityRating { get; set; }

    public double TotalValueForMoneyRating {  get; set; }

    public int NumberOfPeople { get; set; }

    public Food Food { get; set; }
}