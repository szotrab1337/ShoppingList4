using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Entities;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Blazor.Models;
using ShoppingList4.Blazor.Pages.Dialogs;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Index
    {
        [Inject] public ILogger<Index> Logger { get; set; } = default!;
        [Inject] public ITokenService TokenService { get; set; } = default!;
        [Inject] public IShoppingListService ShoppingListService { get; set; } = default!;
        [Inject] public IDialogService DialogService { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        public List<ShoppingList> ShoppingLists { get; set; } = [];
        public bool IsLoading { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            var tokenExists = await CheckUserAsync();

            if (tokenExists)
            {
                await GetShoppingLists();

                Logger.LogInformation("Loaded shopping lists.");
            }
        }

        public async Task<bool> CheckUserAsync()
        {
            if (!await TokenService.Exists())
            {
                NavigationManager.NavigateTo("/Login");
                return false;
            }

            return true;
        }

        private async Task GetShoppingLists()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                ShoppingLists = await ShoppingListService.GetAll();

                IsLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred during loading shopping lists");
            }
        }

        public void Open(int shoppingListId)
        {
            NavigationManager.NavigateTo($"/Entries/{shoppingListId}");
        }

        public async Task Edit(int shoppingListId)
        {
            var shoppingList = ShoppingLists.Find(x => x.Id == shoppingListId);
            if (shoppingList is null)
            {
                return;
            }

            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, shoppingList.Name  },
                { x => x.Title, "Edycja listy zakupów"  }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Edycja listy zakupów",
                parameters,
                Common.GetDialogOptions());

            var dialogResult = await dialog.Result;

            if (!dialogResult.Canceled)
            {
                var data = dialogResult.Data.ToString();
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                shoppingList.Name = data;

                var isUpdated = await ShoppingListService.Update(shoppingList);
                if (isUpdated)
                {
                    await GetShoppingLists();

                    Logger.LogInformation("Updated shopping list with id {id}.", shoppingList.Id);
                    Snackbar.Add("Dokonano edycji listy zakupów!", Severity.Success);
                }
            }
        }

        public async Task Add()
        {
            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, string.Empty  },
                { x => x.Title, "Nowa lista zakupów"  }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Nowa lista zakupów",
                parameters,
                Common.GetDialogOptions());

            var dialogResult = await dialog.Result;

            if (!dialogResult.Canceled)
            {
                var data = dialogResult.Data.ToString();
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                var isAdded = await ShoppingListService.Add(data);
                if (isAdded)
                {
                    await GetShoppingLists();

                    Logger.LogInformation("Added new shopping list with name {name}.", data);
                    Snackbar.Add("Dodano now¹ listê zakupów!", Severity.Success);
                }
            }
        }

        public async Task Delete(int shoppingListId)
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usun¹æ wybran¹ listê zakupów?",
                yesText: "Usuñ", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var isDeleted = await ShoppingListService.Delete(shoppingListId);
            if (isDeleted)
            {
                await GetShoppingLists();

                Logger.LogInformation("User deleted shopping list with id {id}", shoppingListId);
                Snackbar.Add("Lista zakupów zosta³a usuniêta!", Severity.Success);
            }
        }
    }
}