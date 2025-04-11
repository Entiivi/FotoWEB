using FotoKlubasSvetaine.Server.Controllers;
using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
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

// Authentication (Google & GitHub)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["GoogleAuth:ClientId"] ?? throw new ArgumentNullException("GoogleAuth:ClientId is not configured.");
    options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"] ?? throw new ArgumentNullException("GoogleAuth:ClientSecret is not configured.");
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

// Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
logger.LogInformation("Session services configured");

// Database Connection (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 21))));
logger.LogInformation("Database connection established");

// Repositories
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IFotografijaRepository, FotografijaRepository>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", corsBuilder =>
    {
        corsBuilder.WithOrigins(
            "https://localhost:5173",    // React HTTPS
            "https://localhost:7281",    // Razor HTTPS
            "http://localhost:5174",     // React HTTP
            "http://localhost:5281"      // Razor HTTP
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
logger.LogInformation("CORS policy configured");

// Swagger Configuration
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
            Url = new Uri("https://FotoKlubas.com"),
        }
    });
});
logger.LogInformation("Swagger configured");

// Kestrel Port Configuration with Logging
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5001);
    logger.LogInformation("HTTP listening on http://localhost:5001");

    serverOptions.ListenAnyIP(7001, listenOptions =>
    {
        listenOptions.UseHttps();
        logger.LogInformation("HTTPS listening on https://localhost:7001");
    });
});


logger.LogInformation("Kestrel ports configured (5001 HTTP, 7001 HTTPS)");

var app = builder.Build();
logger.LogInformation("Application is building...");

// Middleware pipeline
app.UseSession();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
logger.LogInformation("Middleware configured");

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FotoKlubas API v1");
    c.RoutePrefix = "swagger";
});
logger.LogInformation("Swagger UI available at https://localhost:7001/swagger");

// API Endpoints
app.MapFotografijaEndpoints();
app.MapLoginEndpoints();
app.MapUploadEndpoints();
app.MapUserEndpoints();
app.MapFallbackToFile("index.html");
logger.LogInformation("API Endpoints mapped");

// Static Files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Nuotraukos")),
    RequestPath = "/Nuotraukos",
    ServeUnknownFileTypes = false,
    DefaultContentType = "image/jpeg"
});
logger.LogInformation("Static file serving configured for /Nuotraukos");

// Authentication Endpoints
app.MapGet("/login", async context =>
{
    logger.LogInformation("User attempting Google login...");
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/Main"
    });
});

app.MapGet("/login/github", async (HttpContext context) =>
{
    logger.LogInformation("User attempting GitHub login...");
    await context.ChallengeAsync(GitHubAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "https://localhost:5173/main"
    });
});

app.MapGet("/logout", async (HttpContext context) =>
{
    logger.LogInformation("User logging out...");
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/login");
});

// Port Verification (Logs Active Ports)
var urls = app.Urls;
foreach (var url in urls)
{
    logger.LogInformation($"Application running at: {url}");
}

app.UseWebSockets();
logger.LogInformation("Application is running...");
app.Run();
