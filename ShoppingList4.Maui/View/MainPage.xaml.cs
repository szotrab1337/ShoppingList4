using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class MainPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            _viewModel = vm;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.Initialize();
        }
    }
}