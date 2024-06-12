namespace DataAcessLayer.Entity;

public class Quality
{
    [Key]
    public int Id { get; set; }

    public int QualityValue { get; set; }

    public ICollection<Review> Reviews { get; set; }
}