namespace ShoppingList4.Maui.Interfaces
{
    public interface IAccountService
    {
        Task Login(string email, string password);
    }
}