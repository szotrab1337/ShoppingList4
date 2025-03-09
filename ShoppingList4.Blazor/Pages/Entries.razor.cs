using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Dtos;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Blazor.Models;
using ShoppingList4.Blazor.Pages.Dialogs;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Entries
    {
        [Parameter] public string? Id { get; set; }

        [Inject] public IEntryService EntryService { get; set; } = null!;
        [Inject] public ILogger<Entries> Logger { get; set; } = null!;
        [Inject] public IUserService UserService { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        private List<Entry> EntriesList { get; set; } = [];
        private bool IsLoading { get; set; }

        private int _id;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            if (string.IsNullOrEmpty(Id) || !int.TryParse(Id, out _id))
            {
                return;
            }

            var userExists = await CheckUser();
            if (userExists)
            {
                await GetEntries();

                Logger.LogInformation("Loaded entries.");
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

        private async Task GetEntries()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                EntriesList = [.. (await EntryService.GetShoppingListEntries(_id)).OrderBy(x => x.IsBought)];

                IsLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred during loading shopping lists");
            }
        }

        private async Task Delete(int entryId)
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usunąć wybraną pozycję?",
                yesText: "Usuń", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var result = await EntryService.Delete(entryId);
            if (result)
            {
                var entry = EntriesList.FirstOrDefault(x => x.Id == entryId);
                if (entry is null)
                {
                    return;
                }

                EntriesList.Remove(entry);

                Logger.LogInformation("User deleted entry with id {id}", entryId);
                Snackbar.Add("Pozycja została usunięta!", Severity.Success);
            }
        }

        private async Task ChangeEntryState(int entryId)
        {
            var entry = EntriesList.Find(x => x.Id == entryId);
            if (entry is null)
            {
                return;
            }

            entry.IsBought = !entry.IsBought;

            var dto = new EditEntryDto { Id = entry.Id, IsBought = entry.IsBought, Name = entry.Name };
            await EntryService.Update(dto);

            StateHasChanged();
        }

        private async Task Edit(int entryId)
        {
            var entry = EntriesList.Find(x => x.Id == entryId);
            if (entry is null)
            {
                return;
            }

            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, entry.Name }, { x => x.Title, "Edycja pozycji" }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Edycja pozycji",
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

            var dto = new EditEntryDto { Id = entry.Id, Name = data };
            var result = await EntryService.Update(dto);

            if (result.Id > 0)
            {
                entry.Name = dto.Name;
                StateHasChanged();

                Logger.LogInformation("Updated entry with id {id}.", entry.Id);
                Snackbar.Add("Dokonano edycji pozycji!", Severity.Success);
            }
        }

        public async Task Add()
        {
            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, string.Empty }, { x => x.Title, "Nowa pozycja" }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Nowa pozycja",
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

            var dto = new AddEntryDto { Name = data, ShoppingListId = _id };
            var result = await EntryService.Add(dto);

            if (result.Id > 0)
            {
                EntriesList.Add(result);
                StateHasChanged();

                Logger.LogInformation("Added new entry with name {name} to list {id}.", data, _id);
                Snackbar.Add("Dodano nową pozycję!", Severity.Success);
            }
        }

        public async Task DeleteAll()
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usunąć wszystkie pozycje?",
                yesText: "Usuń", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            await DeleteMultipleAsync(EntriesList.ToList());

            Snackbar.Add("Wszystkie pozycje zostały usunięte!", Severity.Success);
        }

        private async Task DeleteMultipleAsync(List<Entry> entries)
        {
            var entriesIds = entries.ConvertAll(entry => entry.Id);

            var result = await EntryService.DeleteMultiple(entriesIds);
            if (result)
            {
                foreach (var entry in entries)
                {
                    EntriesList.Remove(entry);
                }

                StateHasChanged();

                Logger.LogInformation("User deleted entries with ids {@ids}", entriesIds);
            }
        }

        public async Task DeleteBought()
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usunąć kupione pozycje?",
                yesText: "Usuń", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var entries = EntriesList.Where(x => x.IsBought).ToList();
            await DeleteMultipleAsync(entries);

            Snackbar.Add("Kupione pozycje zostały usunięte!", Severity.Success);
        }
    }
}