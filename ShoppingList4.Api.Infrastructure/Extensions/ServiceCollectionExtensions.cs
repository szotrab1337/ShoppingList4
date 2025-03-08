using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList4.Api.Domain.Interfaces;
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
        }
    }
}