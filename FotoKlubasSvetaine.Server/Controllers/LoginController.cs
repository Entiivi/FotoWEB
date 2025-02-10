using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Login endpoint
        endpoints.MapPost("/login", async (LoginModel model, ILoginRepository loginRepository) =>
        {
            var user = await loginRepository.ValidateUserAsync(model.Username, model.Password);
            if (user != null)
            {
                return Results.Ok(new { Message = "Login successful" });
            }

            return Results.Json(
                new { Message = "Invalid credentials" },
                statusCode: StatusCodes.Status401Unauthorized
            );
        })
        .WithTags("Login") // Swagger tag for grouping
        .WithName("Login"); // Operation ID for Swagger

        // Google Login Endpoint
        endpoints.MapGet("/login/google", async (HttpContext context) =>
        {
            // Challenge the user to log in with Google
            await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = "https://localhost:5173/main" // Where to redirect after Google login
            });
        })
        .WithTags("Login")
        .WithName("GoogleLogin");

        // Google Login Callback Endpoint
        endpoints.MapGet("/google-callback", async (HttpContext context) =>
        {
            // Get user claims from the Google authentication process
            var result = await context.AuthenticateAsync();
            if (result.Succeeded && result.Principal != null)
            {
                var claims = result.Principal.Claims;

                // Extract user details from claims (e.g., email, name)
                var email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
                var name = claims.FirstOrDefault(c => c.Type == "name")?.Value;

                // You can create or update your user in the database here, if needed
                return Results.Ok(new { Message = "Google login successful", Email = email, Name = name });
            }

            return Results.Redirect("/login?error=GoogleLoginFailed");
        })
        .WithTags("Login")
        .WithName("GoogleCallback");
    }
}

