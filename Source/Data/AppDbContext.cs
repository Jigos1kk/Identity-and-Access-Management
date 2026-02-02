using System;
using Microsoft.EntityFrameworkCore;
using Source.Models;

namespace Source.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
}
