namespace DataAcessLayer.Entity;

public class MealPlan
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    [MaxLength(100)]
    public string MealName { get; set; }

    public Food Food { get; set; }
}