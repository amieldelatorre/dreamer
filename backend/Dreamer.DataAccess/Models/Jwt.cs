using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dreamer.DataAccess.Models;
public class Jwt
{
    [Key]
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTime ExpiryData { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTime DateCreated { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTime DateModified { get; set; }

    [Required]
    [Column(TypeName = "boolean")]
    public required Boolean IsDisabled { get; set; }

    //Foreign key for User
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; }
}
