using CommunityToolkit.Mvvm.ComponentModel;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Entry = ShoppingList4.Maui.Entity.Entry;
using System.ComponentModel;

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

            CheckCommand = new RelayCommand<Entry>(Check);
        }

        public IRelayCommand CheckCommand { get; }

        private int _shoppingListId;

        [ObservableProperty]
        private ObservableCollection<Entry> _entries;

        public async void InitializeAsync()
        {
            await GetEntriesAsync();
        }

        private async Task GetEntriesAsync()
        {
            try
            {
                var entries = await _entryService.GetAsync(_shoppingListId);

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
            _shoppingListId = (query["ShoppingList"] as ShoppingList)!.Id;
            InitializeAsync();
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
    }
}
