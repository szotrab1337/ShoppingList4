using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Entities;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Blazor.Models;
using ShoppingList4.Blazor.Pages.Dialogs;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Entries
    {
        [Parameter] public string? Id { get; set; }

        [Inject] public IEntryService EntryService { get; set; } = default!;
        [Inject] public ILogger<Entries> Logger { get; set; } = default!;
        [Inject] public ITokenService TokenService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public IDialogService DialogService { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        public List<Entry> EntriesList { get; set; } = [];
        public bool IsLoading { get; set; }

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

            var tokenExists = await CheckUserAsync();
            if (tokenExists)
            {
                await GetEntries();

                Logger.LogInformation("Loaded entries.");
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

        private async Task GetEntries()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                EntriesList = [.. (await EntryService.Get(_id)).OrderBy(x => x.IsBought)];

                IsLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred during loading shopping lists");
            }
        }

        public async Task Delete(int entryId)
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usun¹æ wybran¹ pozycjê?",
                yesText: "Usuñ", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var isDeleted = await EntryService.Delete(entryId);
            if (isDeleted)
            {
                await GetEntries();

                Logger.LogInformation("User deleted entry with id {id}", entryId);
                Snackbar.Add("Pozycja zosta³a usuniêta!", Severity.Success);
            }
        }

        public async Task ChangeEntryState(int entryId)
        {
            var entry = EntriesList.Find(x => x.Id == entryId);
            if(entry is null)
            {
                return;
            }

            entry.IsBought = !entry.IsBought;
            await EntryService.Update(entry);

            StateHasChanged();
        }

        public async Task Edit(int entryId)
        {
            var entry = EntriesList.Find(x => x.Id == entryId);
            if (entry is null)
            {
                return;
            }

            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, entry.Name  },
                { x => x.Title, "Edycja pozycji"  }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Edycja pozycji",
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

                entry.Name = data;

                var isUpdated = await EntryService.Update(entry);
                if (isUpdated)
                {
                    await GetEntries();

                    Logger.LogInformation("Updated entry with id {id}.", entry.Id);
                    Snackbar.Add("Dokonano edycji pozycji!", Severity.Success);
                }
            }
        }

        public async Task Add()
        {
            var parameters = new DialogParameters<SimpleDialog>
            {
                { x => x.Text, string.Empty  },
                { x => x.Title, "Nowa pozycja"  }
            };

            var dialog = await DialogService.ShowAsync<SimpleDialog>("Nowa pozycja",
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

                var isAdded = await EntryService.Add(data, _id);
                if (isAdded)
                {
                    await GetEntries();

                    Logger.LogInformation("Added new entry with name {name} to list {id}.", data, _id);
                    Snackbar.Add("Dodano now¹ pozycjê!", Severity.Success);
                }
            }
        }

        public async Task DeleteAll()
        {
            bool? isConfirmed = await DialogService.ShowMessageBox(
                "Potwierdzenie",
                "Czy chcesz usun¹æ wszystkie pozycje?",
                yesText: "Usuñ", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            await DeleteMultipleAsync(EntriesList.ToList());

            Snackbar.Add("Wszystkie pozycje zosta³y usuniête!", Severity.Success);
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
                "Czy chcesz usun¹æ kupione pozycje?",
                yesText: "Usuñ", cancelText: "Anuluj");

            if (isConfirmed != true)
            {
                return;
            }

            var entries = EntriesList.Where(x => x.IsBought).ToList();
            await DeleteMultipleAsync(entries);

            Snackbar.Add("Kupione pozycje zosta³y usuniête!", Severity.Success);
        }
    }
}