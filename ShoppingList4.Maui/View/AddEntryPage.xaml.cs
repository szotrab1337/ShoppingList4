using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.View
{
    public partial class AddEntryPage
    {
        public AddEntryPage(AddEntryViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}