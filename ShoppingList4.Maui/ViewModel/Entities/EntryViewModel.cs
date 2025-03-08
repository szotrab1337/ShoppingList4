using CommunityToolkit.Mvvm.ComponentModel;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Maui.ViewModel.Entities
{
    public partial class EntryViewModel : ObservableObject
    {
        public EntryViewModel(Entry entry)
        {
            Id = entry.Id;
            Name = entry.Name;
            IsBought = entry.IsBought;
            ShoppingListId = entry.ShoppingListId;
        }

        public event EventHandler? OnBoughtStatusChanged;

        [ObservableProperty] private int _id;

        [ObservableProperty] private string _name;

        [ObservableProperty] private bool _isBought;

        [ObservableProperty] private int _shoppingListId;

        public void Update(Entry entry)
        {
            Name = entry.Name;
            IsBought = entry.IsBought;
        }

        partial void OnIsBoughtChanged(bool value)
        {
            _ = value;
            
            OnBoughtStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}