using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Models;
using Microsoft.EntityFrameworkCore;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Get user info
        endpoints.MapGet("/user/userinfo", async (string username, ApplicationDbContext context) =>
        {
            var user = await context.Narys.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return Results.NotFound(new { Message = "User not found" });
            }
            return Results.Ok(user);
        })
        .WithTags("User") // Group in Swagger under "User"
        .WithName("GetUserInfo"); // Operation ID for Swagger

        // Create user account
        endpoints.MapPost("/user/create", async (Narys newNarys, ApplicationDbContext context) =>
        {
            context.Narys.Add(newNarys);
            await context.SaveChangesAsync();
            return Results.Ok(new { Message = "Account created successfully." });
        })
        .WithTags("User")
        .WithName("CreateAccount"); // Operation ID for Swagger
    }
}
