namespace DataAcessLayer.Entity;

public class Rating
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    public double RatingValue { get; set; }

    public User User { get; set; }
    public Food Food { get; set; }
}