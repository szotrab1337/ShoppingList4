using CommunityToolkit.Mvvm.ComponentModel;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Maui.ViewModel.Entities
{
    public partial class ShoppingListViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private int _itemsBoughtCount;

        [ObservableProperty]
        private int _itemsCount;

        [ObservableProperty]
        private string _name;

        public ShoppingListViewModel(ShoppingList shoppingList)
        {
            Id = shoppingList.Id;
            Name = shoppingList.Name;
            ItemsCount = shoppingList.ItemsCount;
            ItemsBoughtCount = shoppingList.ItemsBoughtCount;
        }

        public void Update(ShoppingList shoppingList)
        {
            Name = shoppingList.Name;
            ItemsCount = shoppingList.ItemsCount;
            ItemsBoughtCount = shoppingList.ItemsBoughtCount;
        }
    }
}