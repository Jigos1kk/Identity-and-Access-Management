using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Source.Models;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("uuid")]
    public Guid Uuid { get; set; }

    [Column("first_name")]
    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    [MaxLength(255)]
    public string LastName { get; set; } = string.Empty;

    public User()
    {
        Uuid = Guid.NewGuid();
    }
}