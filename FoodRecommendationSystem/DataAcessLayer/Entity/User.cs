using DataAcessLayer.Entity;
using System.Diagnostics.CodeAnalysis;

namespace DataAcessLayer;
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    [Column(TypeName = "varchar(10)")]
    public string Password { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Email { get; set; }

    [Required]
    [MaxLength(80)]
    [Column(TypeName = "varchar(80)")]
    public string Name { get; set; }

    [MaxLength(10)]
    [Column(TypeName = "varchar(10)")]
    public string Gender { get; set; }

    [MaxLength(30)]
    [Column(TypeName = "varchar(30)")]
    [NotNull]
    public string EmpId { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public ICollection<Review> Reviews { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<UserNotification> UserNotifications { get; set; }
    public Role Role { get; set; }
}
