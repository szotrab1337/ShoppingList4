using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class AddShoppingListPage
    {
        public AddShoppingListPage(AddShoppingListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}