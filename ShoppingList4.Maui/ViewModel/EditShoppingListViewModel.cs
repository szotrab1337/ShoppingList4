using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EditShoppingListViewModel : ObservableValidator, IQueryAttributable
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IMessageBoxService _messageBoxService;

        public EditShoppingListViewModel(IShoppingListService shoppingListService, IMessageBoxService messageBoxService)
        {
            _shoppingListService = shoppingListService;
            _messageBoxService = messageBoxService;

            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }

        public IAsyncRelayCommand SaveAsyncCommand { get; }

        [ObservableProperty]
        private ShoppingList _shoppingList;

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

        partial void OnShoppingListChanged(ShoppingList value)
        {
            Name = value.Name;
        }

        private async Task SaveAsync()
        {
            try
            {

                ShoppingList.Name = Name;
                var result = await _shoppingListService.UpdateAsync(ShoppingList);

                if (result)
                {
                    await Shell.Current.GoToAsync("..");
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ShoppingList = (query["ShoppingList"] as ShoppingList)!;
        }
    }
}
