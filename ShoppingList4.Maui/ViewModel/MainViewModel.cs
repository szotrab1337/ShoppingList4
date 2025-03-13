using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.View;
using ShoppingList4.Maui.View.Popups;
using ShoppingList4.Maui.ViewModel.Entities;
using System.Collections.ObjectModel;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class MainViewModel(
        IUserService userService,
        IShoppingListService shoppingListService,
        IMessageBoxService messageBoxService,
        IDialogService dialogService) : ObservableObject
    {
        private readonly IUserService _userService = userService;
        private readonly IShoppingListService _shoppingListService = shoppingListService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;
        private readonly IDialogService _dialogService = dialogService;

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
            var popup = new InputPopup(shoppingList.Name);
            var name = await _dialogService.ShowPromptAsync(popup) as string;

            if (!string.IsNullOrEmpty(name))
            {
                var dto = new EditShoppingListDto { Id = shoppingList.Id, Name = name };
                var result = await _shoppingListService.Update(dto);

                var vm = ShoppingLists.FirstOrDefault(x => x.Id == shoppingList.Id);
                vm?.Update(result);
            }
        }

        [RelayCommand]
        private async Task Add()
        {
            var popup = new InputPopup();
            var name = await _dialogService.ShowPromptAsync(popup) as string;

            if (!string.IsNullOrEmpty(name))
            {
                var dto = new AddShoppingListDto { Name = name };
                var result = await _shoppingListService.Add(dto);

                ShoppingLists.Add(new ShoppingListViewModel(result));
            }
        }

        [RelayCommand]
        private async Task Open(ShoppingListViewModel shoppingList)
        {
            var navigationParam = new Dictionary<string, object> { { "ShoppingListId", shoppingList.Id } };

            await Shell.Current.GoToAsync(nameof(EntriesPage), navigationParam);
        }
    }
}