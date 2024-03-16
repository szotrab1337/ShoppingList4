using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Entity;
using ShoppingList4.Blazor.Interfaces;

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            var tokenExists = await CheckUserAsync();

            if (tokenExists)
            {
                await GetShoppingListsAsync();
                StateHasChanged();

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

        private async Task GetShoppingListsAsync()
        {
            try
            {
                ShoppingLists = await ShoppingListService.GetAll();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred during loading shopping lists");
            }
        }

        public async Task Open()
        {

        }

        public async Task Edit()
        {

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

            var result = await ShoppingListService.Delete(shoppingListId);
            if (result)
            {
                await GetShoppingListsAsync();
                StateHasChanged();

                Logger.LogInformation("User deleted shopping list with id {id}", shoppingListId);
                Snackbar.Add("Lista zakupów zosta³a usuniêta!", MudBlazor.Severity.Success);
            }
        }
    }
}