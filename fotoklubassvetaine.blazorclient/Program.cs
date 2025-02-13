using fotoklubassvetaine.blazorclient;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5000/") });
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddFluentUIComponents();
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
