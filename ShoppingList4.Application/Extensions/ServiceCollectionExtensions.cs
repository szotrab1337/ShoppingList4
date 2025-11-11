using Microsoft.Extensions.DependencyInjection;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Application.Services;

namespace ShoppingList4.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IShoppingListService, ShoppingListService>();
            services.AddScoped<IEntryService, EntryService>();
        }
    }
}