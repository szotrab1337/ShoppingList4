namespace ShoppingList4.Maui.Interfaces
{
    public interface IAppPopupService
    {
        Task<string> ShowInputPopup(string value);
        event EventHandler<bool>? PopupVisibilityChanged;
    }
}