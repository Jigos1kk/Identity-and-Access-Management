using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Source.Data;

namespace Source.ViewModels;

public class UserRequest : IValidatableObject
{
    [Required(ErrorMessage = "Логин обязательно")]
    [MinLength(1, ErrorMessage = "Логин не может быть пустым")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Почта обязательно")]
    [MinLength(1, ErrorMessage = "Почта не может быть пустой")]
    [EmailAddress(ErrorMessage = "Не валидный почтовый адрес")]
    public string? Email { get; set; }


    [Required(ErrorMessage = "Имя обязательно")]
    [MinLength(1, ErrorMessage = "Имя не может быть пустой")]
    public string? FirstName { get; set; }

    [MinLength(1, ErrorMessage = "Фамилия не может быть пустой")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(5, ErrorMessage = "Пароль должен состоять хотябы из 5 символов")]
    public string? Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (!string.IsNullOrEmpty(Email))
        {   
            var dbContext = validationContext.GetService<AppDbContext>();
            if (dbContext != null && dbContext.User.Any(u => u.Email == Email))
            {
                results.Add(new ValidationResult(
                    "Пользователь с такой почтой уже существует", 
                    new[] { nameof(Email) }));
            }
        }

        return results;
    }
}