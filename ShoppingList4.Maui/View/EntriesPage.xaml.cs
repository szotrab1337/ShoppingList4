using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View;

public partial class EntriesPage : ContentPage
{
    public EntriesPage(EntriesViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}