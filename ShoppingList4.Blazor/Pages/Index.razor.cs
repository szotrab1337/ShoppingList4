using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Dtos;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Blazor.Models;
using ShoppingList4.Blazor.Pages.Dialogs;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Index
    {
        [Inject] public ILogger<Index> Logger { get; set; } = null!;
        [Inject] public IUserService UserService { get; set; } = null!;
        [Inject] public IShoppingListService ShoppingListService { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public ISnackbar Snackbar { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private List<ShoppingList> ShoppingLists { get; set; } = [];
        private bool IsLoading { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            var userExists = await CheckUser();

            if (userExists)
            {
                await GetShoppingLists();
            }
        }

        private async Task<bool> CheckUser()
        {
            if (await UserService.ExistsCurrentUser())
            {
                return true;
            }

            NavigationManager.NavigateTo("/Login");
            return false;
        }

        private async Task GetShoppingLists()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                ShoppingLists = (await ShoppingListService.GetAll()).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while loading shopping lists");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private void Open(int shoppingListId)
        {
            NavigationManager.NavigateTo($"/Entries/{shoppingListId}");
        }

        private async Task Edit(int shoppingListId)
        {
            var shoppingList = ShoppingLists.Find(x => x.Id == shoppingListId);
            if (shoppingList is null)
            {
                return;
            }

            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, shoppingList.Name }, { x => x.Title, "Edycja listy zakupów" }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Edycja listy zakupów",
                parameters,
                Common.GetDialogOptions());

            var dialogResult = await dialog.Result;

            if (dialogResult is { Canceled: true } or null)
            {
                return;
            }

            var data = dialogResult.Data?.ToString();
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var dto = new EditShoppingListDto { Id = shoppingListId, Name = data };
            var result = await ShoppingListService.Update(dto);

            if (result.Id > 0)
            {
                shoppingList.Name = result.Name;
                StateHasChanged();

                Logger.LogInformation("Updated shopping list with id {id}.", shoppingList.Id);
                Snackbar.Add("Dokonano edycji listy zakupów!", Severity.Success);
            }
        }

        public async Task Add()
        {
            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, string.Empty }, { x => x.Title, "Nowa lista zakupów" }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Nowa lista zakupów",
                parameters,
                Common.GetDialogOptions());

            var dialogResult = await dialog.Result;

            if (dialogResult is { Canceled: true } or null)
            {
                return;
            }

            var data = dialogResult.Data?.ToString();
            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var dto = new AddShoppingListDto { Name = data };
            var result = await ShoppingListService.Add(dto);

            if (result.Id > 0)
            {
                ShoppingLists.Add(result);
                StateHasChanged();

                Logger.LogInformation("Added new shopping list with name {name}.", data);
                Snackbar.Add("Dodano nową listę zakupów!", Severity.Success);
            }
        }

        private async Task Delete(int shoppingListId)
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usunąć wybraną listę zakupów?",
                yesText: "Usuń", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var result = await ShoppingListService.Delete(shoppingListId);
            if (result)
            {
                var shoppingList = ShoppingLists.FirstOrDefault(x => x.Id == shoppingListId);
                if (shoppingList is null)
                {
                    return;
                }

                ShoppingLists.Remove(shoppingList);

                Logger.LogInformation("User deleted shopping list with id {id}", shoppingListId);
                Snackbar.Add("Lista zakupów została usunięta!", Severity.Success);
            }
        }
    }
}