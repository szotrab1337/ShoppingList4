using CommunityToolkit.Maui.Core;

namespace ShoppingList4.Maui.View.Popups
{
    public partial class InputPopup
    {
        public InputPopup(string? value = null)
        {
            InitializeComponent();

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            InputEntry.Text = value;
            InputEntry.CursorPosition = InputEntry.Text.Length;
        }

        private async void OnOkClicked(object sender, EventArgs e)
        {
            await CloseAsync(InputEntry.Text, CancellationToken.None);
        }

        private async void InputPopup_OnOpened(object? sender, PopupOpenedEventArgs e)
        {
            await Task.Delay(150);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                InputEntry.Focus();
            });
        }
    }
}