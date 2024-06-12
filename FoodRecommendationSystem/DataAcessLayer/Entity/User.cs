using DataAcessLayer.Entity;

namespace DataAcessLayer;
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Password { get; set; }

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(10)]
    public string Gender { get; set; }

    [MaxLength(50)]
    public string EmpId { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public ICollection<Review> Reviews { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public Role Role { get; set; }
}
