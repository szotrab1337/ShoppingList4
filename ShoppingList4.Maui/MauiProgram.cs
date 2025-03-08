using CommunityToolkit.Maui;
using DevExpress.Maui;
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
                .UseDevExpress(useLocalization: false)
                .UseDevExpressControls()
                .UseDevExpressCollectionView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.UseMauiApp<App>().UseMauiCommunityToolkit();

            builder.Services.AddInfrastructure();
            builder.Services.AddApplication();

            return builder.Build();
        }
    }
}