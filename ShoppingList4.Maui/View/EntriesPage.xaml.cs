using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class EntriesPage
    {
        private readonly EntriesViewModel _viewModel;

        public EntriesPage(EntriesViewModel vm)
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