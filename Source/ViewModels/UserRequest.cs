using System;
using System.ComponentModel.DataAnnotations;

namespace Source.ViewModels;

public class UserRequest /* : IValidatableObject */
{
    [Required(ErrorMessage = "Имя обязательно")]
    [MinLength(1, ErrorMessage = "Имя не может быть пустой")]
    public string? FirstName { get; set; }

    [MinLength(1, ErrorMessage = "Фамилия не может быть пустой")]
    public string? LastName { get; set; }
}