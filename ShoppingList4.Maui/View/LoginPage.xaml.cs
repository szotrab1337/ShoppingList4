using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;

        BindingContext = _viewModel;
    }

    private readonly LoginPageViewModel _viewModel;

    protected override async void OnAppearing()
    {
        await Task.Delay(400);
        await _viewModel.InitializeAsync();
    }
}