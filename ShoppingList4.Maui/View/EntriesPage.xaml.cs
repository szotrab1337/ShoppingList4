using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View;

public partial class EntriesPage : ContentPage
{
    public EntriesPage(EntriesViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;

        BindingContext = _viewModel;
    }

    private readonly EntriesViewModel _viewModel;

    protected override void OnAppearing()
    {
        _viewModel.InitializeAsync();
    }
}