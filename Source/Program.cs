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
builder.Services.Configure<SmtpOption>(builder.Configuration.GetSection(SmtpOption.Smtp));
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection"));
});

builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<User>(options =>
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

var app = builder.Build();
app.UseRouting();
app.UseRequestLocalization();

app.UseAuthorization();
app.MapIdentityApi<User>();

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
