using CommunityToolkit.Maui.Views;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class DialogService : IDialogService
    {
        private readonly Page? _page = Application.Current?.MainPage;

        public async Task<object?> ShowPromptAsync(Popup popup)
        {
            return _page != null ? await _page.ShowPopupAsync(popup) : null;
        }
    }
}