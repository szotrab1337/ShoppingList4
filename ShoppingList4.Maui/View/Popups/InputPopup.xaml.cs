using ShoppingList4.Maui.ViewModel.Popups;

namespace ShoppingList4.Maui.View.Popups
{
    public partial class InputPopup
    {
        public InputPopup(InputPopupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void InputPopup_OnOpened(object? sender, EventArgs e)
        {
            await Task.Delay(250);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                InputEntry.CursorPosition = InputEntry.Text.Length;
                InputEntry.Focus();
            });
        }
    }
}