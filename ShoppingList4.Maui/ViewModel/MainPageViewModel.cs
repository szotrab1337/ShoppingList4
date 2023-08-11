﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using System.Collections.ObjectModel;
using ShoppingList4.Maui.View;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly ITokenService _tokenService;
        private readonly IShoppingListService _shoppingListService;

        public MainPageViewModel(ITokenService tokenService, IShoppingListService shoppingListService)
        {
            _tokenService = tokenService;
            _shoppingListService = shoppingListService;

            AddAsyncCommand = new AsyncRelayCommand(AddAsync);
            RefreshAsyncCommand = new AsyncRelayCommand(RefreshAsync);
            DeleteAsyncCommand = new AsyncRelayCommand<ShoppingList>(DeleteAsync);

            Check();
            Initialize();
        }

        public IAsyncRelayCommand AddAsyncCommand { get; }
        public IAsyncRelayCommand RefreshAsyncCommand { get; }
        public IAsyncRelayCommand DeleteAsyncCommand { get; }

        [ObservableProperty]
        private ObservableCollection<ShoppingList> _shoppingLists = new();

        [ObservableProperty]
        private bool _isRefreshing;

        public async void Initialize()
        {
            await GetShoppingLists();
        }

        private async Task RefreshAsync()
        {
            await GetShoppingLists();
            IsRefreshing = false;
        }

        private async Task DeleteAsync(ShoppingList shoppingList)
        {
            if (shoppingList is null)
            {
                return;
            }

            var confirmation = await Application.Current?.MainPage?.DisplayAlert("Potwierdzenie",
                "Czy na pewno chcesz usunąć wybraną listę?", "TAK", "NIE")!;

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

        private async Task GetShoppingLists()
        {
            try
            {
                var shoppingLists = await _shoppingListService.GetAll();

                if (shoppingLists is null || !shoppingLists.Any())
                {
                    return;
                }

                ShoppingLists = new ObservableCollection<ShoppingList>(shoppingLists);
            }
            catch (Exception)
            {
                await Application.Current?.MainPage?.DisplayAlert("Błąd",
                    "Wystąpił błąd. Spróbuj ponownie.", "OK")!;
            }
        }

        private async void Check()
        {
            if (!await _tokenService.ExistsAsync())
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        }

        private async Task AddAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddShoppingListPage));
        }
    }
}
