using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Source.Data;

namespace Source.ViewModels;

public class UserRequest /* : IValidatableObject */
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
    public string? Password { get; set; }
}