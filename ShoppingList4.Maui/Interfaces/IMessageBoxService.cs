namespace ShoppingList4.Maui.Interfaces;

public interface IMessageBoxService
{
    Task<bool> ShowAlert(string title, string message, string accept, string cancel);
    Task ShowAlert(string title, string message, string cancel);
}