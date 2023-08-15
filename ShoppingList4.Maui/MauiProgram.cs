using CommunityToolkit.Maui;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Services;
using ShoppingList4.Maui.View;
using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.UseMauiApp<App>().UseMauiCommunityToolkit();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddScoped<MainPageViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageViewModel>();

            builder.Services.AddTransient<AddShoppingListPage>();
            builder.Services.AddTransient<AddShoppingListViewModel>();

            builder.Services.AddTransient<EditShoppingListPage>();
            builder.Services.AddTransient<EditShoppingListViewModel>();

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
            builder.Services.AddScoped<IMessageBoxService, MessageBoxService>();

            builder.Services.AddHttpClient("ShoppingList4", x =>
            {
                x.BaseAddress = new Uri("http://192.168.1.100:88/");
                x.Timeout = TimeSpan.FromSeconds(5);
            });

            return builder.Build();
        }
    }
}