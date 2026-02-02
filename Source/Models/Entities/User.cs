using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Source.Models;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; }
    
    [Column("uuid")]
    public Guid Uuid { get; }

    [Column("first_name")]
    [Required] 
    public string FirstName { get; set;}

    [Column("last_name")]
    public string LastName { get; set;}
}
