using ShoppingList4.Maui.View;

namespace ShoppingList4.Maui
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(EntriesPage), typeof(EntriesPage));
        }
    }
}