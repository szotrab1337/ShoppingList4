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
            services.AddTransient<LoginPageViewModel>();

            services.AddTransient<AddShoppingListPage>();
            services.AddTransient<AddShoppingListViewModel>();

            services.AddTransient<EditShoppingListPage>();
            services.AddTransient<EditShoppingListViewModel>();

            services.AddTransient<EntriesPage>();
            services.AddTransient<EntriesViewModel>();

            services.AddTransient<AddEntryPage>();
            services.AddTransient<AddEntryViewModel>();

            services.AddTransient<EditEntryPage>();
            services.AddTransient<EditEntryViewModel>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShoppingListService, ShoppingListService>();
            services.AddScoped<IEntryService, EntryService>();
            services.AddScoped<IMessageBoxService, MessageBoxService>();
        }
    }
}