using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShoppingList4.Api;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Middlewares;
using ShoppingList4.Api.Models;
using ShoppingList4.Api.Models.Validators;
using ShoppingList4.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var authenticationSettings = builder.Configuration.GetSection("Authentication").Get<AuthenticationSettings>();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Destructure.ByTransforming<ShoppingList>(x => new {x.Id, x.Name, x.CreatedOn})
    .Destructure.ByTransforming<Entry>(x => new {x.Id, x.Name, x.CreatedOn, x.ShoppingListId})
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddSingleton(authenticationSettings!);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings!.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

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
builder.Services.AddScoped<IValidator<CreateEntryDto>, CreateEntryDtoValidator>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IDbManager, DbManager>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbManager = scope.ServiceProvider.GetService<IDbManager>();
dbManager?.Migrate();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingList4 Api");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
