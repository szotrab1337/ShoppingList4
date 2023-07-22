using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ShoppingListDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingListDbConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
