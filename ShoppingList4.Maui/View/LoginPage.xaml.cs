using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}