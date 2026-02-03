using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using Source.Data;
using Source.Models;
using Source.ViewModels;

namespace Source.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserController(this RouteGroupBuilder groups)
    {
        groups.MapGet("/", GetUsers).Produces(200);
        groups.MapGet("/{uuid}", GetUsersById).Produces(200).ProducesProblem(404);
        groups.MapPost("/", CreateUser).Accepts<UserRequest>("application/json").Produces(201).ProducesProblem(400);

        return groups;
    }

    internal static async Task<IResult> GetUsers([FromServices] AppDbContext context)
    {
        var users = await context.User.ToListAsync();
        var userResponses = users.Select(u => new UserResponce(u.Uuid, u.FirstName, u.LastName)).ToList();
    
        return TypedResults.Ok(userResponses);
    }

    internal static async Task<IResult> GetUsersById([FromServices] AppDbContext context, [FromRoute] Guid uuid)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Uuid == uuid);
        return user is User user1 ? TypedResults.Ok(new UserResponce(user.Uuid, user.FirstName, user.LastName)) : TypedResults.NotFound();
    }

    internal static async Task<IResult> CreateUser([FromServices] AppDbContext context, [FromBody] UserRequest userRequest)
    {
        var newUser = new User
        {
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName
        };

        context.User.AddAsync(newUser);
        await context.SaveChangesAsync();

        return TypedResults.Created($"/users/{newUser.Uuid}", new UserResponce(newUser.Uuid, newUser.FirstName, newUser.LastName));
    }
}
