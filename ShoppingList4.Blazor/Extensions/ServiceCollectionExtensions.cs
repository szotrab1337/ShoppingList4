using MudBlazor.Services;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Blazor.Services;

namespace ShoppingList4.Blazor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShoppingListService, ShoppingListService>();
            services.AddScoped<IEntryService, EntryService>();
            services.AddMudServices();
        }
    }
}