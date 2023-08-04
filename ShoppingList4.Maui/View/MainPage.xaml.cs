using CommunityToolkit.Mvvm.DependencyInjection;
using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = Ioc.Default.GetRequiredService<MainPageViewModel>();
        }
    }
}