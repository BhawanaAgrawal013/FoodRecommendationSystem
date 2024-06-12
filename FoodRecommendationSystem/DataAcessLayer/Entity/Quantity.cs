namespace DataAcessLayer.Entity;

public class Quantity
{
    [Key]
    public int Id { get; set; }

    public int QuantityValue { get; set; }

    public ICollection<Review> Reviews { get; set; }
}