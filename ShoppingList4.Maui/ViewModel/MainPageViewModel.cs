using CommunityToolkit.Mvvm.ComponentModel;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly ITokenService _tokenService;

        public MainPageViewModel(ITokenService tokenService)
        {
            _tokenService = tokenService;

            Check();
        }

        private async void Check()
        {
            if (!await _tokenService.ExistsAsync())
            {
                await Shell.Current.GoToAsync("LoginPage");
            }
        }
    }
}
