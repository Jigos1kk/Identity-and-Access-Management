using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using Source.Data;
using Source.Models.Entities;
using Source.Service;
using Source.ViewModels;

namespace Source.Endpoints;

public static class UserEndpoints
{

    public static RouteGroupBuilder MapUserController(this RouteGroupBuilder groups)
    {
        groups.MapGet("/", GetUsers).Produces(200);
        groups.MapGet("/{uuid}", GetUsersById).RequireAuthorization().Produces(200).ProducesProblem(404).ProducesProblem(403);
        groups.MapPost("/", CreateUser).Accepts<UserRequest>("application/json").Produces(201).ProducesProblem(400);

        return groups;
    }

    internal static async Task<IResult> GetUsers([FromServices] AppDbContext context)
    {
        var users = await context.User.ToListAsync();
        var userResponses = users.Select(u => new UserResponce(u)).ToList();
    
        return TypedResults.Ok(userResponses);
    }

    internal static async Task<IResult> GetUsersById([FromServices] AppDbContext context, [FromRoute] Guid uuid)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Uuid == uuid);
        return user is User user1 ? TypedResults.Ok(new UserResponce(user)) : TypedResults.NotFound();
    }

    internal static async Task<IResult> CreateUser(
        [FromServices] UserManager<User> userManager, 
        [FromServices] IEmailSender emailSender, 
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromServices] LinkGenerator linkGenerator,
        [FromBody] UserRequest userRequest)
    {

        var newUser = new User
        {
            UserName = userRequest.UserName, 
            Email = userRequest.Email,
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName
        };

        var results = await userManager.CreateAsync(newUser, userRequest.Password);

        if (results.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
            
            var httpContext = httpContextAccessor.HttpContext;
            var request = httpContext.Request;
            var confirmationLink = $"{request.Scheme}://{request.Host}/user/confirm-email?userId={newUser.Uuid}&token={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))}";

            await emailSender.SendEmailAsync(
                newUser.Email, 
                "Подтвердите ваш email", 
                $"Пожалуйста, подтвердите ваш email, перейдя по ссылке: <a href='{confirmationLink}'>Подтвердить email</a>"
            );
            return TypedResults.Created($"/users/{newUser.Uuid}", new UserResponce(newUser));
        }

        return TypedResults.BadRequest(results.Errors);
    }
}
