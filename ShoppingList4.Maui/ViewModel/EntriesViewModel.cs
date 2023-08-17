using CommunityToolkit.Mvvm.ComponentModel;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Entry = ShoppingList4.Maui.Entity.Entry;
using System.ComponentModel;
using ShoppingList4.Maui.View;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EntriesViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IEntryService _entryService;
        private readonly IMessageBoxService _messageBoxService;

        public EntriesViewModel(IEntryService entryService, IMessageBoxService messageBoxService)
        {
            _entryService = entryService;
            _messageBoxService = messageBoxService;

            AddAsyncCommand = new AsyncRelayCommand(AddAsync);
            CheckCommand = new RelayCommand<Entry>(Check);
            RefreshAsyncCommand = new AsyncRelayCommand(RefreshAsync);
            DeleteAsyncCommand = new AsyncRelayCommand<Entry>(DeleteAsync);
            DeleteAllAsyncCommand = new AsyncRelayCommand(DeleteAllAsync);
            DeleteBoughtAsyncCommand = new AsyncRelayCommand(DeleteBoughtAsync);
        }

        public IAsyncRelayCommand AddAsyncCommand { get; }
        public IRelayCommand CheckCommand { get; }
        public IAsyncRelayCommand RefreshAsyncCommand { get; }
        public IAsyncRelayCommand DeleteAsyncCommand { get; }
        public IAsyncRelayCommand DeleteAllAsyncCommand { get; }
        public IAsyncRelayCommand DeleteBoughtAsyncCommand { get; }

        private ShoppingList _shoppingList;

        [ObservableProperty]
        private ObservableCollection<Entry> _entries;

        [ObservableProperty]
        private bool _isRefreshing;

        public async void InitializeAsync()
        {
            await GetEntriesAsync();
        }

        private async Task RefreshAsync()
        {
            await GetEntriesAsync();
            IsRefreshing = false;
        }

        private async Task GetEntriesAsync()
        {
            try
            {
                var entries = await _entryService.GetAsync(_shoppingList.Id);

                Entries = new ObservableCollection<Entry>(entries);
                Entries.ToList().ForEach(x => x.PropertyChanged += EntryPropertyChanged);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _shoppingList = query["ShoppingList"] as ShoppingList;
        }

        private void Check(Entry entry)
        {
            entry.IsBought = !entry.IsBought;
        }

        //triggers also on Check method
        private async void EntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsBought")
                {
                    var entry = (Entry)sender;
                    var result = await _entryService.UpdateAsync(entry);

                    if (!result)
                    {
                        entry.IsBought = !entry.IsBought;
                    }
                }
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private async Task DeleteAsync(Entry entry)
        {
            try
            {
                if (entry is null)
                {
                    return;
                }

                var result = await _entryService.DeleteAsync(entry.Id);
                if (result)
                {
                    entry.PropertyChanged -= EntryPropertyChanged;
                    Entries.Remove(entry);
                }
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }
        
        private async Task DeleteAllAsync()
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
        
        private async Task DeleteBoughtAsync()
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

        private async Task DeleteMultipleAsync(List<Entry> entries)
        {
            var entriesIds = entries.Select(entry => entry.Id).ToList();

            var result = await _entryService.DeleteMultipleAsync(entriesIds);
            if (result)
            {
                foreach (var entry in entries)
                {
                    entry.PropertyChanged -= EntryPropertyChanged;
                    Entries.Remove(entry);
                }
            }
        }

        private async Task AddAsync()
        {
            var navigationParam = new Dictionary<string, object>
            {
                { "ShoppingList", _shoppingList }
            };

            await Shell.Current.GoToAsync(nameof(AddEntryPage), navigationParam);
        }
    }
}
