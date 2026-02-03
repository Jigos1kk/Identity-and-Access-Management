using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Source.Data;
using Source.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
    app.MapScalarApiReference("/api-docs");
}

app.MapHealthChecks("/health");
app.MapGroup("/user").MapUserController();

app.Run();
