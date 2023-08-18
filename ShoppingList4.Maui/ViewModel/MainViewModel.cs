using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using System.Collections.ObjectModel;
using ShoppingList4.Maui.View;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ITokenService _tokenService;
        private readonly IShoppingListService _shoppingListService;
        private readonly IMessageBoxService _messageBoxService;

        public MainViewModel(ITokenService tokenService, IShoppingListService shoppingListService,
            IMessageBoxService messageBoxService)
        {
            _tokenService = tokenService;
            _shoppingListService = shoppingListService;
            _messageBoxService = messageBoxService;

            AddAsyncCommand = new AsyncRelayCommand(AddAsync);
            RefreshAsyncCommand = new AsyncRelayCommand(RefreshAsync);
            DeleteAsyncCommand = new AsyncRelayCommand<ShoppingList>(DeleteAsync);
            EditAsyncCommand = new AsyncRelayCommand<ShoppingList>(EditAsync);
            OpenShoppingListAsyncCommand = new AsyncRelayCommand<ShoppingList>(OpenShoppingListAsync);
        }

        public IAsyncRelayCommand AddAsyncCommand { get; }
        public IAsyncRelayCommand RefreshAsyncCommand { get; }
        public IAsyncRelayCommand DeleteAsyncCommand { get; }
        public IAsyncRelayCommand EditAsyncCommand { get; }
        public IAsyncRelayCommand OpenShoppingListAsyncCommand { get; }

        [ObservableProperty]
        private ObservableCollection<ShoppingList> _shoppingLists = new();

        [ObservableProperty]
        private bool _isRefreshing;

        public async Task InitializeAsync()
        {
            await GetShoppingListsAsync();
        }

        private async Task RefreshAsync()
        {
            await GetShoppingListsAsync();
            IsRefreshing = false;
        }

        private async Task DeleteAsync(ShoppingList shoppingList)
        {
            try
            {
                if (shoppingList is null)
                {
                    return;
                }

                var confirmation = await _messageBoxService.ShowAlert("Potwierdzenie",
                    "Czy na pewno chcesz usunąć wybraną listę?", "TAK", "NIE");

                if (!confirmation)
                {
                    return;
                }

                var result = await _shoppingListService.DeleteAsync(shoppingList.Id);
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

        private async Task GetShoppingListsAsync()
        {
            try
            {
                var shoppingLists = await _shoppingListService.GetAllAsync();

                ShoppingLists = new ObservableCollection<ShoppingList>(shoppingLists);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        public async Task<bool> CheckUserAsync()
        {
            if (!await _tokenService.ExistsAsync())
            {
                await Shell.Current.GoToAsync("//Login");
                return false;
            }

            return true;
        }

        private async Task EditAsync(ShoppingList shoppingList)
        {
            var navigationParam = new Dictionary<string, object>
            {
                { "ShoppingList", shoppingList }
            };

            await Shell.Current.GoToAsync(nameof(EditShoppingListPage), navigationParam);
        }

        private async Task AddAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddShoppingListPage));
        }

        private async Task OpenShoppingListAsync(ShoppingList shoppingList)
        {
            var navigationParam = new Dictionary<string, object>
            {
                { "ShoppingList", shoppingList }
            };

            await Shell.Current.GoToAsync(nameof(EntriesPage), navigationParam);
        }
    }
}
