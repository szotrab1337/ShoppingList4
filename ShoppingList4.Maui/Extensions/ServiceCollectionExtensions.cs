using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Services;
using ShoppingList4.Maui.View;
using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<MainPage>();
            services.AddSingleton<MainViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();

            services.AddTransient<EntriesPage>();
            services.AddTransient<EntriesViewModel>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShoppingListService, ShoppingListService>();
            services.AddScoped<IEntryService, EntryService>();
            services.AddScoped<IMessageBoxService, MessageBoxService>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddScoped<INavigationService, NavigationService>();
        }
    }
}