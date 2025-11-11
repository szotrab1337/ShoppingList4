using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.View.Popups;

namespace ShoppingList4.Maui.Services
{
    public class DialogService(IApplication application) : IDialogService
    {
        private readonly IApplication _application = application;

        public async Task<string?> ShowInputPopup(string? value = null)
        {
            var popup = new InputPopup(value);
            await ShowPromptAsync(popup);
            return await popup.ResultTask;
        }

        public async Task<object?> ShowPromptAsync(Popup popup)
        {
            var window = _application.Windows[0];
            if (window.Content is not Page page)
            {
                throw new InvalidOperationException();
            }

            return await page.ShowPopupAsync(popup);
        }
    }
}