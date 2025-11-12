using CommunityToolkit.Maui;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Services;
using ShoppingList4.Maui.View;
using ShoppingList4.Maui.View.Popups;
using ShoppingList4.Maui.ViewModel;
using ShoppingList4.Maui.ViewModel.Popups;

namespace ShoppingList4.Maui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMaui(this IServiceCollection services)
        {
            services.AddSingleton<MainPage>();
            services.AddSingleton<MainViewModel>();

            services.AddTransient<LoginPage>();
            services.AddTransient<LoginViewModel>();

            services.AddTransient<EntriesPage>();
            services.AddTransient<EntriesViewModel>();

            services.AddSingleton<IAppPopupService, AppPopupService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransientPopup<InputPopup, InputPopupViewModel>();
        }
    }
}