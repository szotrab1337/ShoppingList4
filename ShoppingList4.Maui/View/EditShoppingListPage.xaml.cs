using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View;

public partial class EditShoppingListPage : ContentPage
{
    public EditShoppingListPage(EditShoppingListViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}