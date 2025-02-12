using FotoKlubasSvetaine.Server.Controllers;
using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using AspNet.Security.OAuth.GitHub;

var builder = WebApplication.CreateBuilder(args);

// Add Authentication (Google Login)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleAuth:ClientId").Value ?? throw new ArgumentNullException("GoogleAuth:ClientId is not configured.");
    options.ClientSecret = builder.Configuration.GetSection("GoogleAuth:ClientSecret").Value ?? throw new ArgumentNullException("GoogleAuth:ClientSecret is not configured.");
    options.SaveTokens = true;
    options.CallbackPath = "/signin-google";
})
.AddGitHub(options =>
 {
     options.ClientId = builder.Configuration["GitHubAuth:ClientId"];
     options.ClientSecret = builder.Configuration["GitHubAuth:ClientSecret"];
     options.CallbackPath = "/signin-github"; // GitHub callback path
     options.SaveTokens = true;
 });


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Required for non-EU cookie policies
});


// Configure cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/login";  // Path for login
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

// Add services to the container

// Configure database connection (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 21))));

// Add repositories to the DI container
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IFotografijaRepository, FotografijaRepository>();

// Add controllers
builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("https://localhost:5173, https://localhost:5001") // Frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});


// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FotoKlubas API",
        Description = "An ASP.NET Core Web API for managing photo club",
        Contact = new OpenApiContact
        {
            Name = "FotoKlubas",
            Email = string.Empty,
            Url = new Uri("https://FotoKlubas.com"),
        }
    });

});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Custom header for token
    options.Cookie.Name = "X-CSRF-TOKEN"; // Cookie for anti-forgery validation
    options.Cookie.HttpOnly = true; // Ensure it's secure
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
});


var app = builder.Build();

app.UseSession();
// Configure the middleware pipeline

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();


// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable routing
app.UseRouting();

// Enable CORS
app.UseCors("AllowFrontend");

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();



// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FotoKlubas API v1");
    c.RoutePrefix = "swagger"; // Swagger UI available at /swagger
});

// Map Minimal API endpoints
app.MapFotografijaEndpoints();
app.MapLoginEndpoints();
app.MapUploadEndpoints();
app.MapUserEndpoints();


app.MapGet("/antiforgery-token", async (IAntiforgery antiforgery, HttpContext context) =>
{
    var tokens = antiforgery.GetAndStoreTokens(context);
    return Results.Json(new { token = tokens.RequestToken });
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Nuotraukos")),
    RequestPath = "/Nuotraukos"
});


// Add Google Login routes
app.MapGet("/login", async context =>
{
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/"
    });
});


// GitHub Login Route
app.MapGet("/login/github", async (HttpContext context) =>
{
    await context.ChallengeAsync(GitHubAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "https://localhost:5173/main"
    });
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/"); // Redirect to the login page or homepage
});


app.UseWebSockets();

// Run the application
app.Run();
