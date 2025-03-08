using ShoppingList4.Api.Application.Extensions;
using ShoppingList4.Api.Domain.Interfaces;
using ShoppingList4.Api.Extensions;
using ShoppingList4.Api.Infrastructure.Extensions;
using ShoppingList4.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLogger(builder.Configuration);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddVersioning();
builder.Services.AddMiddlewares();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbManager = scope.ServiceProvider.GetService<IDbManager>();
dbManager?.Migrate();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
