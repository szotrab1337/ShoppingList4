using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class LoginPageViewModel : ObservableValidator
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public LoginPageViewModel(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;

            LoginAsyncCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
            LogoutCommand = new RelayCommand(Logout);

            Initialize();
        }

        public IAsyncRelayCommand LoginAsyncCommand { get; }
        public IRelayCommand LogoutCommand { get; }

        [ObservableProperty]
        private bool _tokenExists;

        [EmailAddress(ErrorMessage = "Nie jest to poprawny adres email.")]
        [Required(ErrorMessage = "Pole wymagane")]
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                ValidateProperty(_email);
                LoginAsyncCommand.NotifyCanExecuteChanged();
            }
        }
        private string _email;

        [Required(ErrorMessage = "Pole wymagane")]
        [MinLength(6, ErrorMessage = "Podaj minimum 6 znaków")]
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ValidateProperty(_password);
                LoginAsyncCommand.NotifyCanExecuteChanged();
            } 
        }
        private string _password;

        private async void Initialize()
        {
            TokenExists = await _tokenService.ExistsAsync();
        }

        private async Task LoginAsync()
        {
            var user = new User(Email, Password);
            await _accountService.LoginAsync(user);

            if (!await _tokenService.ExistsAsync())
            {
                await Application.Current?.MainPage?.DisplayAlert("Błąd", "Niepoprawne dane.", "OK")!;
                return;
            }

            await Shell.Current.GoToAsync("..");
        }

        private void Logout()
        {
            _tokenService.Remove();
            TokenExists = false;
        }

        private bool CanLogin()
        {
            ValidateAllProperties();

            return !HasErrors;
        }
    }
}
