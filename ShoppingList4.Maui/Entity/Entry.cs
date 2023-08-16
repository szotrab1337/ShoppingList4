using CommunityToolkit.Mvvm.ComponentModel;

namespace ShoppingList4.Maui.Entity
{
    public class Entry : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool IsBought
        {
            get => _isBought;
            set => SetProperty(ref _isBought, value);
        }
        private bool _isBought;

        public int ShoppingListId { get; set; }
    }
}
