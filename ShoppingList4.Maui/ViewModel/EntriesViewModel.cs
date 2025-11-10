using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Application.Dtos;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel.Entities;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EntriesViewModel(
        IEntryService entryService,
        IMessageBoxService messageBoxService,
        IDialogService dialogService) : ObservableObject, IQueryAttributable
    {
        private readonly IDialogService _dialogService = dialogService;
        private readonly IEntryService _entryService = entryService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;

        [ObservableProperty]
        private ObservableCollection<EntryViewModel> _entries = [];

        [ObservableProperty]
        private bool _isInitializing;

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _loaded;
        private int _shoppingListId;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _shoppingListId = (int)query["ShoppingListId"];

            if (query.TryGetValue("ShoppingListId", out var shoppingListIdObj) &&
                shoppingListIdObj is int shoppingListId)
            {
                _shoppingListId = shoppingListId;
            }
        }

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

            _entryService.EntryAdded += OnEntryAdded;
            _entryService.EntryDeleted += OnEntryDeleted;
            _entryService.EntryUpdated += OnEntryUpdated;
        }

        [RelayCommand]
        private async Task Add()
        {
            var name = await _dialogService.ShowInputPopup();

            if (!string.IsNullOrEmpty(name))
            {
                var dto = new AddEntryDto { ShoppingListId = _shoppingListId, Name = name };
                await _entryService.Add(dto);
            }
        }

        [RelayCommand]
        private async Task ChangeIsBoughtValue(EntryViewModel? viewModel)
        {
            if (viewModel is null)
            {
                return;
            }

            var dto = new EditEntryDto
            {
                Id = viewModel.Id,
                IsBought = viewModel.IsBought,
                Name = viewModel.Name
            };

            await _entryService.Update(dto);
        }

        [RelayCommand]
        private async Task Delete(EntryViewModel entry)
        {
            try
            {
                await _entryService.Delete(entry.Id);
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
            var entriesIds = entries.Select(x => x.Id).ToList();

            await _entryService.DeleteMultiple(entriesIds);
        }

        [RelayCommand]
        private async Task Edit(EntryViewModel entry)
        {
            var name = await _dialogService.ShowInputPopup(entry.Name);

            if (!string.IsNullOrEmpty(name))
            {
                var dto = new EditEntryDto { Id = entry.Id, Name = name };
                await _entryService.Update(dto);
            }
        }

        private async Task GetEntries()
        {
            try
            {
                var entries = await _entryService.GetShoppingListEntries(_shoppingListId);
                var vms = entries
                    .Select(entry => new EntryViewModel(entry, ChangeIsBoughtValueCommand))
                    .OrderBy(x => x.IsBought);

                Entries = new ObservableCollection<EntryViewModel>(vms);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private void OnEntryAdded(object? sender, Entry e)
        {
            var vm = new EntryViewModel(e, ChangeIsBoughtValueCommand);
            Entries.Insert(0, vm);
        }

        private void OnEntryDeleted(object? sender, int e)
        {
            var entry = Entries.FirstOrDefault(x => x.Id == e);
            if (entry is null)
            {
                return;
            }

            Entries.Remove(entry);
        }

        private void OnEntryUpdated(object? sender, Entry e)
        {
            var vm = Entries.FirstOrDefault(x => x.Id == e.Id);
            vm?.Update(e);
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await GetEntries();
            IsRefreshing = false;
        }
    }
}