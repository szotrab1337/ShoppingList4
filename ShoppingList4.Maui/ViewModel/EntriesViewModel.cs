using CommunityToolkit.Mvvm.ComponentModel;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using System.Collections.ObjectModel;
using Entry = ShoppingList4.Maui.Entity.Entry;

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
        }

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
    }
}
