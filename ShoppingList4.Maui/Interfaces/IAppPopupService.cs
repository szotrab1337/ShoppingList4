namespace ShoppingList4.Maui.Interfaces
{
    public interface IAppPopupService
    {
        Task<string> ShowInputPopup(string value);
    }
}