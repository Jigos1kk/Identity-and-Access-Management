using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Source.Data;
using Source.Models;

namespace Source.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public UserController(AppDbContext dbContext) => _dbContext = dbContext;

    [HttpGet]
    public async Task<List<User>> Get()
    {
        return await _dbContext.User.ToListAsync();
    }
    
}
