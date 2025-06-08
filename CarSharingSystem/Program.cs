using CarSharingSystem.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
//Add CarSharingContext
builder.Services.AddDbContext<CarSharingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();
app.MapOpenApi();

// Add Scalar
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarSharingContext>();
    context.Database.EnsureCreated(); // Stworzy bazê i zaaplikuje seed
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();