namespace ShoppingList4.Maui.View.Popups
{
    public partial class InputPopup
    {
        public InputPopup(string? value = null)
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(value))
            {
                InputEntry.Text = value;
            }
        }

        private async void OnOkClicked(object sender, EventArgs e)
        {
            await CloseAsync(InputEntry.Text, CancellationToken.None);
        }
    }
}