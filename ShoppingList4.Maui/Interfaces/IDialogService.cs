using CommunityToolkit.Maui.Views;

namespace ShoppingList4.Maui.Interfaces
{
    public interface IDialogService
    {
        Task<object?> ShowPromptAsync(Popup popup);
        Task<string?> ShowInputPopup(string? value = null);
    }
}