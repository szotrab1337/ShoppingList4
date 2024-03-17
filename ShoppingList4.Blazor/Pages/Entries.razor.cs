using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Entities;
using ShoppingList4.Blazor.Interfaces;

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
                StateHasChanged();

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
                EntriesList = (await EntryService.Get(_id))
                    .OrderBy(x => x.IsBought)
                    .ToList();
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
                StateHasChanged();

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

        }

        public async Task Add()
        {

        }
    }
}