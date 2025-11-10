using Microsoft.Extensions.DependencyInjection;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Infrastructure.Repositories;

namespace ShoppingList4.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // private const string _baseAddress = "http://192.168.1.24:88/api/v1/";
        private const string BaseAddress = "https://shopping-list-4-app.azurewebsites.net/api/v1/";

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddHttpClient("ShoppingList4Api", x =>
            {
                x.BaseAddress = new Uri(BaseAddress);
                x.Timeout = TimeSpan.FromSeconds(15);
            });
        }
    }
}