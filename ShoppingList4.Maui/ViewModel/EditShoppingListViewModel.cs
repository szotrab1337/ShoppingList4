using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel.Entities;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EditShoppingListViewModel(
        IShoppingListService shoppingListService,
        IMessageBoxService messageBoxService) : ObservableValidator, IQueryAttributable
    {
        private readonly IShoppingListService _shoppingListService = shoppingListService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;

        private int _id;

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
                var dto = new EditShoppingListDto { Id = _id, Name = Name };
                var result = await _shoppingListService.Update(dto);

                var navigationParam = new Dictionary<string, object> { { "EditedShoppingList", result } };
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
            var shoppingList = (query["ShoppingList"] as ShoppingListViewModel)!;

            _id = shoppingList.Id;
            Name = shoppingList.Name;
        }
    }
}