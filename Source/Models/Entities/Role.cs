using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Source.Models.Entities;

[Table("Roles")]
public class Role : IdentityRole<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public override int Id { get; set; }
    
    [Column("Uuid")]
    public Guid Uuid { get; set; }

    [Column("Name")]
    [Required]
    [NotNull]
    public override string Name {get; set;}

    [Column("Descripton")]
    public string? Descripton {get; set;}


    public Role() : base()
    {
        Uuid = Guid.NewGuid();
    }
}
