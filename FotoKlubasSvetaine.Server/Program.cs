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
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var logger = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
}).CreateLogger("Startup");

logger.LogInformation("Starting application...");

builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "Nuotraukos" // Set the Nuotraukos folder as the web root
});

logger.LogInformation("Web root set to 'Nuotraukos'");

// Add Authentication (Google & GitHub Login)
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
    options.CallbackPath = "/signin-github";
    options.SaveTokens = true;
});

logger.LogInformation("Authentication configured");

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

logger.LogInformation("Session services configured");

// Configure database connection (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 21))));

logger.LogInformation("Database connection established");

// Add repositories to DI container
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

logger.LogInformation("Controllers and Razor Pages configured");

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("https://localhost:5173", "https://localhost:5027")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

logger.LogInformation("CORS policy configured");

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
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

logger.LogInformation("Swagger configured");

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5027);
    serverOptions.ListenAnyIP(7295, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});


var app = builder.Build();

logger.LogInformation("Application is starting...");

// Middleware pipeline
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseStaticFiles();

logger.LogInformation("Middleware configured");

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FotoKlubas API v1");
    c.RoutePrefix = "swagger";
});

logger.LogInformation("Swagger UI available at /swagger");

// Map API endpoints
app.MapFotografijaEndpoints();
app.MapLoginEndpoints();
app.MapUploadEndpoints();
app.MapUserEndpoints();

logger.LogInformation("Endpoints mapped");

// Static file serving
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Nuotraukos")),
    RequestPath = "/Nuotraukos",
    ServeUnknownFileTypes = false,
    DefaultContentType = "image/jpeg"
});

logger.LogInformation("Static file serving configured for /Nuotraukos");

// Google Login
app.MapGet("/login", async context =>
{
    logger.LogInformation("User attempting Google login...");
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/"
    });
});

// GitHub Login
app.MapGet("/login/github", async (HttpContext context) =>
{
    logger.LogInformation("User attempting GitHub login...");
    await context.ChallengeAsync(GitHubAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "https://localhost:5173/main"
    });
});

// Logout
app.MapGet("/logout", async (HttpContext context) =>
{
    logger.LogInformation("User logging out...");
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.UseWebSockets();

logger.LogInformation("Application is running...");
app.Run();
