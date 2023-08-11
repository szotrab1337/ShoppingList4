using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;

            BindingContext = _viewModel;
        }

        private readonly MainPageViewModel _viewModel;

        protected override void OnAppearing()
        {
            _viewModel.Initialize();
        }
    }
}