using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CarSharingSystem.Client;
using CarSharingSystem.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using Paket;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7112/api/") // adres API
});

builder.Services.AddScoped<CarSharingSystem.Client.Services.AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<LocalStorage>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
