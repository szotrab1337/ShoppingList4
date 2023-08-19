using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;

            BindingContext = _viewModel;
        }

        private readonly MainViewModel _viewModel;

        protected override async void OnAppearing()
        {
            await _viewModel.InitializeAsync();   
        }
    }
}