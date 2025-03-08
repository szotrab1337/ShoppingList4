using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class AddEntryViewModel(
        IEntryService entryService,
        IMessageBoxService messageBoxService) : ObservableValidator, IQueryAttributable
    {
        private readonly IEntryService _entryService = entryService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;

        private int _shoppingListId;

        [NotifyDataErrorInfo]
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [MaxLength(35, ErrorMessage = "Wprowadzona nazwa jest zbyt długa")]
        [Required(ErrorMessage = "Pole wymagane")]
        private string _name = string.Empty;

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task Save()
        {
            try
            {
                var dto = new AddEntryDto { ShoppingListId = _shoppingListId, Name = Name };
                var result = await _entryService.Add(dto);

                var navigationParam = new Dictionary<string, object> { { "AddedEntry", result } };
                await Shell.Current.GoToAsync("..", navigationParam);
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _shoppingListId = (int)query["ShoppingListId"];
        }
    }
}