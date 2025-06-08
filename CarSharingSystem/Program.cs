using CarSharingSystem.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
//Add CarSharingContext
builder.Services.AddDbContext<CarSharingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();
app.MapOpenApi();

// Add Scalar
app.MapScalarApiReference();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarSharingContext>();
    context.Database.EnsureCreated(); // Stworzy bazê i zaaplikuje seed
}

app.Run();