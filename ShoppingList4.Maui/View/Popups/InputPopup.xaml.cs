namespace ShoppingList4.Maui.View.Popups
{
    public partial class InputPopup
    {
        private readonly TaskCompletionSource<string> _tcs = new();

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
        
        public Task<string> ResultTask => _tcs.Task;

        private async void OnOkClicked(object sender, EventArgs e)
        {
            _tcs.SetResult(InputEntry.Text);
            await CloseAsync(CancellationToken.None);
        }

        private async void InputPopup_OnOpened(object? sender, EventArgs e)
        {
            await Task.Delay(150);
            MainThread.BeginInvokeOnMainThread(() => { InputEntry.Focus(); });
        }
    }
}