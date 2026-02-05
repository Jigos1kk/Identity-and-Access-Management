using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Source.Models;
using Source.Models.Entities;

namespace Source.Data;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<IdentityUserClaim<int>> UserClaims { get; set; }
    public DbSet<IdentityUserRole<int>> UserRole { get; set; }

}
