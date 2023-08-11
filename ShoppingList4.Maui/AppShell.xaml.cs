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
        }
    }
}