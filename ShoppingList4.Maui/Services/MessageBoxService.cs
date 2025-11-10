using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class MessageBoxService(IApplication application) : IMessageBoxService
    {
        private readonly IApplication _application = application;

        public async Task<bool> ShowAlert(string title, string message, string accept, string cancel)
        {
            var window = _application.Windows[0];
            if (window.Content is not Page page)
            {
                throw new InvalidOperationException();
            }

            return await page.DisplayAlert(title,
                message, accept, cancel)!;
        }

        public async Task ShowAlert(string title, string message, string cancel)
        {
            var window = _application.Windows[0];
            if (window.Content is not Page page)
            {
                throw new InvalidOperationException();
            }

            await page.DisplayAlert(title,
                message, cancel)!;
        }
    }
}