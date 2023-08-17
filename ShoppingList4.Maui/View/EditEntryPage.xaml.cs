using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View;

public partial class EditEntryPage : ContentPage
{
    public EditEntryPage(EditEntryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}