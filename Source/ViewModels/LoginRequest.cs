using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Source.Data;

namespace Source.ViewModels;

public class LoginRequest
{
    [Required(ErrorMessage = "Почта обязательно")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    public string? Password { get; set; }

    public bool RememberMe { get; set; } = false;
}