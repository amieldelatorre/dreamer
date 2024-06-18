﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dreamer.DataAccess.Models;
public class Jwt
{
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTime ExpiryDate { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTime DateCreated { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public required DateTime DateModified { get; set; }

    [Required]
    [Column(TypeName = "boolean")]
    public required Boolean IsDisabled { get; set; }

    [Column(TypeName = "timestamp with time zone")]
    public DateTime? DateDisabled { get; set; }


    //Foreign key for User
    [ForeignKey("UserId")]
    public required Guid UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}
