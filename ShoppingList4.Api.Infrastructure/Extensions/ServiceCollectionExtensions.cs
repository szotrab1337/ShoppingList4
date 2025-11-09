using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShoppingList4.Api.Application.Interfaces;
using ShoppingList4.Api.Domain.Interfaces;
using ShoppingList4.Api.Infrastructure.Authentication;
using ShoppingList4.Api.Infrastructure.Persistence;
using ShoppingList4.Api.Infrastructure.Repositories;
using ShoppingList4.Api.Infrastructure.Utilities;

namespace ShoppingList4.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppingListDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("ShoppingListDbConnection")),
                ServiceLifetime.Transient);

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEntryRepository, EntryRepository>();
            services.AddTransient<IShoppingListRepository, ShoppingListRepository>();
            services.AddScoped<IDbManager, DbManager>();

            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            services.Configure<JwtSettings>(jwtSettingsSection);

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecurityKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer
                    };
                });
        }
    }
}