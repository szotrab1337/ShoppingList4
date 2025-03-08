using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public async Task<bool> ShowAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current?.MainPage?.DisplayAlert(title,
                message, accept: accept, cancel: cancel)!;
        }

        public async Task ShowAlert(string title, string message, string cancel)
        {
            await Application.Current?.MainPage?.DisplayAlert(title,
                message, cancel: cancel)!;
        }
    }
}
