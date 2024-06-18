using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Dreamer.DataAccess.Models
{
    public class User
    {
        [Key]
        [Column(TypeName = "uuid")]
        public required Guid Id { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public required string FirstName { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public required string LastName { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public required string Email { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public required string Password { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public required DateTime DateCreated { get; set; }

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public required DateTime DateModified { get; set; }

        public ICollection<Jwt> Jwts { get; set; }
    }
}
