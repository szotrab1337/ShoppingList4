using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Middlewares;
using ShoppingList4.Api.Models;
using ShoppingList4.Api.Models.Validators;
using ShoppingList4.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Destructure.ByTransforming<ShoppingList>(x => new {x.Id, x.Name, x.CreatedOn})
    .Destructure.ByTransforming<Entry>(x => new {x.Id, x.Name, x.CreatedOn, x.ShoppingListId})
    .CreateLogger();

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddDbContext<ShoppingListDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingListDbConnection"));
});
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IValidator<ShoppingListDto>, ShoppingListDtoValidator>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingList4 Api");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
