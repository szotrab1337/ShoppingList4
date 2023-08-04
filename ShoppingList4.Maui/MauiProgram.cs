using System.ComponentModel.Design;
using CommunityToolkit.Mvvm.DependencyInjection;
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

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddTransient<MainPageViewModel>()
                    .BuildServiceProvider());

            return builder.Build();
        }
    }
}