using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Application.Dtos;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.View;
using ShoppingList4.Maui.ViewModel.Entities;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class MainViewModel(
        IUserService userService,
        IShoppingListService shoppingListService,
        IMessageBoxService messageBoxService,
        IAppPopupService appPopupService,
        INavigationService navigationService) : ObservableObject
    {
        private readonly IAppPopupService _appPopupService = appPopupService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IShoppingListService _shoppingListService = shoppingListService;
        private readonly IUserService _userService = userService;

        [ObservableProperty]
        private bool _isInitializing;

        [ObservableProperty]
        private bool _isPopupVisible;

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _loaded;

        [ObservableProperty]
        private ObservableCollection<ShoppingListViewModel> _shoppingLists = [];

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
            else
            {
                await _navigationService.NavigateTo("//Login");
            }

            IsInitializing = false;
            _loaded = true;

            _shoppingListService.ShoppingListAdded += OnShoppingListAdded;
            _shoppingListService.ShoppingListDeleted += OnShoppingListDeleted;
            _shoppingListService.ShoppingListUpdated += OnShoppingListUpdated;
            _appPopupService.PopupVisibilityChanged += OnPopupVisibilityChanged;
        }

        [RelayCommand]
        private async Task Add()
        {
            var result = await _appPopupService.ShowInputPopup(string.Empty);
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            var dto = new AddShoppingListDto { Name = result };
            await _shoppingListService.Add(dto);
        }

        private async Task<bool> CheckUser()
        {
            return await _userService.ExistsCurrentUser();
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

                await _shoppingListService.Delete(shoppingList.Id);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        [RelayCommand]
        private async Task Edit(ShoppingListViewModel shoppingList)
        {
            var result = await _appPopupService.ShowInputPopup(shoppingList.Name);
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            var dto = new EditShoppingListDto { Id = shoppingList.Id, Name = result };
            await _shoppingListService.Update(dto);
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

        private void OnPopupVisibilityChanged(object? sender, bool e)
        {
            IsPopupVisible = e;
        }

        private void OnShoppingListAdded(object? sender, ShoppingList e)
        {
            ShoppingLists.Add(new ShoppingListViewModel(e));
        }

        private void OnShoppingListDeleted(object? sender, int e)
        {
            var shoppingList = ShoppingLists.FirstOrDefault(x => x.Id == e);

            if (shoppingList is not null)
            {
                ShoppingLists.Remove(shoppingList);
            }
        }

        private void OnShoppingListUpdated(object? sender, ShoppingList e)
        {
            var vm = ShoppingLists.FirstOrDefault(x => x.Id == e.Id);
            vm?.Update(e);
        }

        [RelayCommand]
        private async Task Open(ShoppingListViewModel shoppingList)
        {
            var navigationParam = new Dictionary<string, object> { { "ShoppingListId", shoppingList.Id } };

            await _navigationService.NavigateTo(nameof(EntriesPage), navigationParam);
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await LoadShoppingLists();
        }
    }
}