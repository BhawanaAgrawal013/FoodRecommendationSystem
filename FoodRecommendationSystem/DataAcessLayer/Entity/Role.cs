namespace DataAcessLayer.Entity;

public class Role
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    [Column(TypeName = "varchar(10)")]
    public string RoleName { get; set; }

    public ICollection<User> Users { get; set; }
}