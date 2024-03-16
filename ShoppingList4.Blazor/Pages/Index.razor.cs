using ShoppingList4.Blazor.Entity;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Index
    {
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
            }
        }

        public async Task<bool> CheckUserAsync()
        {
            if (!await _tokenService.Exists())
            {
                _navigationManager.NavigateTo("/Login");
                return false;
            }

            return true;
        }

        private async Task GetShoppingListsAsync()
        {
            try
            {
                ShoppingLists = await _shoppingListService.GetAll();
            }
            catch (Exception)
            {
                //TODO: handle error
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
            bool? isConfirmed = await _dialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usun¹æ wybran¹ listê zakupów?",
                yesText: "Usuñ", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var result = await _shoppingListService.Delete(shoppingListId);
            if (result)
            {
                await GetShoppingListsAsync();
                StateHasChanged();

                _snackbar.Add("Lista zakupów zosta³a usuniêta!", MudBlazor.Severity.Success);
            }
        }
    }
}