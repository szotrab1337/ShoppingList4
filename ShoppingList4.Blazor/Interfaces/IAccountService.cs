namespace ShoppingList4.Blazor.Interfaces
{
    public interface IAccountService
    {
        Task Login(string email, string password);
    }
}