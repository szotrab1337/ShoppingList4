using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList4.Maui.ViewModel
{
    public class AddShoppingListViewModel : ObservableValidator
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IMessageBoxService _messageBoxService;

        public AddShoppingListViewModel(IShoppingListService shoppingListService, IMessageBoxService messageBoxService)
        {
            _shoppingListService = shoppingListService;
            _messageBoxService = messageBoxService;

            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }

        public IAsyncRelayCommand SaveAsyncCommand { get; }

        [MaxLength(35, ErrorMessage = "Wprowadzona nazwa jest zbyt długa")]
        [Required(ErrorMessage = "Pole wymagane")]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                ValidateProperty(_name);
                SaveAsyncCommand.NotifyCanExecuteChanged();
            }
        }
        private string _name;

        private async Task SaveAsync()
        {
            try
            {
                var result = await _shoppingListService.AddAsync(Name);

                if (result)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private bool CanSave()
        {
            ValidateAllProperties();

            return !HasErrors;
        }
    }
}
