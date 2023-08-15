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

        public EditShoppingListViewModel(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;

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
                var result = await _shoppingListService.Update(ShoppingList);

                if (result)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
            catch (Exception)
            {
                await Application.Current?.MainPage?.DisplayAlert("Błąd",
                    "Wystąpił błąd. Spróbuj ponownie.", "OK")!;
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
