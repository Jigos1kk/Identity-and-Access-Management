using System.Globalization;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Source;
using Source.Data;
using Source.Endpoints;
using Source.Models;
using Source.Models.Entities;
using Source.Options;
using Source.Service;
using Source.Settings;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IJsonLocalizer, JsonLocalizer>();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.AddRequestLocalization(options =>
{
    var cultureList = builder.Configuration.GetSection("Localization").GetSection("SupportedCultures").Get<string[]>()
        .Select(c => new CultureInfo(c))
        .ToList();

    options.DefaultRequestCulture = new RequestCulture(builder.Configuration.GetSection("Localization")["DefaultCulture"]);
    options.SupportedCultures = cultureList;
    options.SupportedUICultures = cultureList;
    
    options.RequestCultureProviders.Insert(0, 
        new QueryStringRequestCultureProvider { QueryStringKey = "culture" });
});

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<JwtService>();
builder.Services.Configure<SmtpOption>(builder.Configuration.GetSection(SmtpOption.Smtp));
builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(JwtOption.Jwt));
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        ValidIssuer = builder.Configuration[$"{JwtOption.Jwt}:Issure"],
        ValidAudience = builder.Configuration[$"{JwtOption.Jwt}:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration[$"{JwtOption.Jwt}:Secret"])),
        
        // NameClaimType = ClaimTypes.NameIdentifier, // Откуда брать NameIdentifier
        // RoleClaimType = ClaimTypes.Role // Откуда брать роли
    };
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();
// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
    app.MapScalarApiReference("/api-docs");
}

app.MapHealthChecks("/health");
app.MapGroup("/user").MapUserController();
app.MapGet("/test", () => "Hello World")
   .RequireAuthorization();

app.MapRazorPages();

app.Run();
