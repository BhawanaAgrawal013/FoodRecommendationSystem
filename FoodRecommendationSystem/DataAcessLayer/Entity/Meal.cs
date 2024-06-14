namespace DataAcessLayer.Entity;

public class Meal
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Food")]
    public int FoodId { get; set; }

    [ForeignKey("MealName")]
    public int MealNameId { get; set; }

    public Food Food { get; set; }

    public MealName MealName { get; set; }
}