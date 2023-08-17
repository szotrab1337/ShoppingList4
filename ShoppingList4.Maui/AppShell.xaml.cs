using ShoppingList4.Maui.View;

namespace ShoppingList4.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AddShoppingListPage), typeof(AddShoppingListPage));
            Routing.RegisterRoute(nameof(EditShoppingListPage), typeof(EditShoppingListPage));
            Routing.RegisterRoute(nameof(EntriesPage), typeof(EntriesPage));
            Routing.RegisterRoute(nameof(AddEntryPage), typeof(AddEntryPage));
            Routing.RegisterRoute(nameof(EditEntryPage), typeof(EditEntryPage));
        }
    }
}