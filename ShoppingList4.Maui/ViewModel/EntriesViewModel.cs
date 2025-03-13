using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core.Internal;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.View.Popups;
using ShoppingList4.Maui.ViewModel.Entities;
using System.Collections.ObjectModel;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EntriesViewModel(
        IEntryService entryService,
        IMessageBoxService messageBoxService,
        IDialogService dialogService) : ObservableObject, IQueryAttributable
    {
        private readonly IEntryService _entryService = entryService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;
        private readonly IDialogService _dialogService = dialogService;

        private int _shoppingListId;

        private bool _loaded;

        [ObservableProperty] private ObservableCollection<EntryViewModel> _entries = [];

        [ObservableProperty] private bool _isRefreshing;

        [ObservableProperty] private bool _isInitializing;

        public async Task Initialize()
        {
            if (_loaded)
            {
                return;
            }

            IsInitializing = true;

            await Task.Delay(400);
            await GetEntries();

            IsInitializing = false;
            _loaded = true;
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await GetEntries();
            IsRefreshing = false;
        }

        private async Task GetEntries()
        {
            try
            {
                var entries = await _entryService.GetShoppingListEntries(_shoppingListId);
                var vms = entries
                    .Select(entry => new EntryViewModel(entry))
                    .OrderBy(x => x.IsBought);

                Entries = new ObservableCollection<EntryViewModel>(vms);
                Entries.ForEach(x => x.OnBoughtStatusChanged += OnEntryBoughtStatusChanged);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private async void OnEntryBoughtStatusChanged(object? sender, EventArgs e)
        {
            if (sender is not EntryViewModel entry)
            {
                return;
            }

            var dto = new EditEntryDto { Id = entry.Id, IsBought = entry.IsBought, Name = entry.Name };
            await _entryService.Update(dto);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _shoppingListId = (int)query["ShoppingListId"];

            if (query.TryGetValue("ShoppingListId", out var shoppingListIdObj) &&
                shoppingListIdObj is int shoppingListId)
            {
                _shoppingListId = shoppingListId;
            }
        }

        [RelayCommand]
        private async Task Delete(EntryViewModel entry)
        {
            try
            {
                var result = await _entryService.Delete(entry.Id);
                if (result)
                {
                    entry.OnBoughtStatusChanged -= OnEntryBoughtStatusChanged;
                    Entries.Remove(entry);
                }
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteAll()
        {
            try
            {
                var confirmation = await _messageBoxService.ShowAlert("Potwierdzenie",
                    "Czy na pewno chcesz usunąć wszystkie pozycje?", "TAK", "NIE");

                if (!confirmation)
                {
                    return;
                }

                await DeleteMultipleAsync(Entries.ToList());
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteBought()
        {
            try
            {
                var confirmation = await _messageBoxService.ShowAlert("Potwierdzenie",
                    "Czy na pewno chcesz usunąć kupione pozycje?", "TAK", "NIE");

                if (!confirmation)
                {
                    return;
                }

                var entries = Entries.Where(x => x.IsBought).ToList();
                await DeleteMultipleAsync(entries);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private async Task DeleteMultipleAsync(List<EntryViewModel> entries)
        {
            var entriesIds = entries.Select(x => x.Id);

            var result = await _entryService.DeleteMultiple(entriesIds);
            if (result)
            {
                foreach (var entry in entries)
                {
                    entry.OnBoughtStatusChanged -= OnEntryBoughtStatusChanged;
                    Entries.Remove(entry);
                }
            }
        }

        [RelayCommand]
        private async Task Add()
        {
            var popup = new InputPopup();
            var name = await _dialogService.ShowPromptAsync(popup) as string;

            if (!string.IsNullOrEmpty(name))
            {
                var dto = new AddEntryDto { ShoppingListId = _shoppingListId, Name = name };
                var result = await _entryService.Add(dto);

                Entries.Insert(0, new EntryViewModel(result));
            }
        }

        [RelayCommand]
        private async Task Edit(EntryViewModel entry)
        {
            var popup = new InputPopup(entry.Name);
            var name = await _dialogService.ShowPromptAsync(popup) as string;

            if (!string.IsNullOrEmpty(name))
            {
                var dto = new EditEntryDto { Id = entry.Id, Name = name };
                var result = await _entryService.Update(dto);

                var vm = Entries.FirstOrDefault(x => x.Id == entry.Id);
                vm?.Update(result);
            }
        }
    }
}