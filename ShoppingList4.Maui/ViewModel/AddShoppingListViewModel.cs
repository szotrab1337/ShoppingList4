using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class AddShoppingListViewModel(
        IShoppingListService shoppingListService,
        IMessageBoxService messageBoxService) : ObservableValidator
    {
        private readonly IShoppingListService _shoppingListService = shoppingListService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;

        [NotifyDataErrorInfo]
        [ObservableProperty]
        [MaxLength(35, ErrorMessage = "Wprowadzona nazwa jest zbyt długa")]
        [Required(ErrorMessage = "Pole wymagane")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _name = string.Empty;

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task Save()
        {
            try
            {
                var dto = new AddShoppingListDto { Name = Name };
                var result = await _shoppingListService.Add(dto);

                var navigationParam = new Dictionary<string, object> { { "AddedShoppingList", result } };
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
    }
}