namespace DataAcessLayer.Entity;

public class Appearance
{
    [Key]
    public int Id { get; set; }

    public int AppearanceValue { get; set; }

    public ICollection<Review> Reviews { get; set; }
}