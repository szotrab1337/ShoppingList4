using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Maui.ViewModel.Entities
{
    public partial class EntryViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private bool _isBought;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private int _shoppingListId;

        public EntryViewModel(Entry entry, IRelayCommand<EntryViewModel> changeIsBoughtCommand)
        {
            Id = entry.Id;
            Name = entry.Name;
            IsBought = entry.IsBought;
            ShoppingListId = entry.ShoppingListId;

            ChangeIsBoughtCommand = changeIsBoughtCommand;
        }

        private IRelayCommand<EntryViewModel>? ChangeIsBoughtCommand { get; }

        public void Update(Entry entry)
        {
            Name = entry.Name;
            IsBought = entry.IsBought;
        }

        partial void OnIsBoughtChanged(bool value)
        {
            _ = value;

            ChangeIsBoughtCommand?.Execute(this);
        }
    }
}