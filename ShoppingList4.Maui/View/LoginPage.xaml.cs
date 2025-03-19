using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;

            BindingContext = _viewModel;
        }

        private readonly LoginViewModel _viewModel;

        protected override async void OnAppearing()
        {
            await Task.Delay(400);
            await _viewModel.Initialize();
        }
    }
}