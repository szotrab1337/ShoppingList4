using CommunityToolkit.Maui;
using DevExpress.Maui;
using ShoppingList4.Application.Extensions;
using ShoppingList4.Infrastructure.Extensions;
using ShoppingList4.Maui.Extensions;

namespace ShoppingList4.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseDevExpress(false)
                .UseDevExpressControls()
                .UseDevExpressCollectionView()
                .UseDevExpressEditors()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddInfrastructure();
            builder.Services.AddMaui();
            builder.Services.AddApplication();

            return builder.Build();
        }
    }
}