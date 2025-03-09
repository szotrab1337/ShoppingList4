using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.View;
using ShoppingList4.Maui.ViewModel.Entities;
using System.Collections.ObjectModel;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class MainViewModel(
        IUserService userService,
        IShoppingListService shoppingListService,
        IMessageBoxService messageBoxService) : ObservableObject, IQueryAttributable
    {
        private readonly IUserService _userService = userService;
        private readonly IShoppingListService _shoppingListService = shoppingListService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;

        private bool _loaded;

        [ObservableProperty] private ObservableCollection<ShoppingListViewModel> _shoppingLists = [];

        [ObservableProperty] private bool _isRefreshing;

        [ObservableProperty] private bool _isInitializing;

        public async Task Initialize()
        {
            if (_loaded)
            {
                return;
            }

            IsInitializing = true;

            await Task.Delay(400);
            var userExists = await CheckUser();

            if (userExists)
            {
                await LoadShoppingLists();
            }

            IsInitializing = false;
            _loaded = true;
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await LoadShoppingLists();
        }

        [RelayCommand]
        private async Task Delete(ShoppingListViewModel shoppingList)
        {
            try
            {
                var confirmation = await _messageBoxService.ShowAlert("Potwierdzenie",
                    "Czy na pewno chcesz usunąć wybraną listę?", "TAK", "NIE");

                if (!confirmation)
                {
                    return;
                }

                var result = await _shoppingListService.Delete(shoppingList.Id);
                if (result)
                {
                    ShoppingLists.Remove(shoppingList);
                }
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private async Task LoadShoppingLists()
        {
            try
            {
                var shoppingLists = await _shoppingListService.GetAll();
                var vms = shoppingLists.Select(x => new ShoppingListViewModel(x));

                ShoppingLists = new ObservableCollection<ShoppingListViewModel>(vms);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task<bool> CheckUser()
        {
            if (await _userService.ExistsCurrentUser())
            {
                return true;
            }

            await Shell.Current.GoToAsync("//Login");
            return false;
        }

        [RelayCommand]
        private async Task Edit(ShoppingListViewModel shoppingList)
        {
            var navigationParam = new Dictionary<string, object> { { "ShoppingList", shoppingList } };

            await Shell.Current.GoToAsync(nameof(EditShoppingListPage), navigationParam);
        }

        [RelayCommand]
        private async Task Add()
        {
            await Shell.Current.GoToAsync(nameof(AddShoppingListPage));
        }

        [RelayCommand]
        private async Task Open(ShoppingListViewModel shoppingList)
        {
            var navigationParam = new Dictionary<string, object> { { "ShoppingListId", shoppingList.Id } };

            await Shell.Current.GoToAsync(nameof(EntriesPage), navigationParam);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("EditedShoppingList", out var editedShoppingListObj) &&
                editedShoppingListObj is ShoppingList editedShoppingList)
            {
                var vm = ShoppingLists.FirstOrDefault(x => x.Id == editedShoppingList.Id);
                vm?.Update(editedShoppingList);
                
                query.Remove("EditedShoppingList");
            }

            if (query.TryGetValue("AddedShoppingList", out var addedShoppingListObj) &&
                addedShoppingListObj is ShoppingList addedShoppingList)
            {
                ShoppingLists.Add(new ShoppingListViewModel(addedShoppingList));
                
                query.Remove("AddedShoppingList");
            }
        }
    }
}