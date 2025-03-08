using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class LoginPageViewModel(IAccountService accountService, IUserService userService,
        IMessageBoxService messageBoxService) : ObservableValidator
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IUserService _userService = userService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;


        [ObservableProperty]
        private bool _userExists;

        [ObservableProperty]
        [EmailAddress(ErrorMessage = "Nie jest to poprawny adres email.")]
        [Required(ErrorMessage = "Pole wymagane")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _email;

        [ObservableProperty]
        [Required(ErrorMessage = "Pole wymagane")]
        [MinLength(6, ErrorMessage = "Podaj minimum 6 znaków")]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _password;

        public async Task Initialize()
        {
            Email = string.Empty;
            Password = string.Empty;
            UserExists = await _userService.ExistsCurrentUser();
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task Login()
        {
            try
            {
                await _accountService.Login(Email, Password);

                if (!await _userService.ExistsCurrentUser())
                {
                    await _messageBoxService.ShowAlert("Błąd", "Niepoprawne dane.", "OK");
                    return;
                }

                await Shell.Current.GoToAsync("//Main");
            }
            catch (Exception ex)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        [RelayCommand]
        private void Logout()
        {
            _userService.RemoveCurrentUser();
            UserExists = false;
        }

        private bool CanLogin()
        {
            ValidateAllProperties();

            return !HasErrors;
        }
    }
}
