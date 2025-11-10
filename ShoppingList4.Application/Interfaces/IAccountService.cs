namespace ShoppingList4.Application.Interfaces
{
    public interface IAccountService
    {
        Task Login(string email, string password);
    }
}