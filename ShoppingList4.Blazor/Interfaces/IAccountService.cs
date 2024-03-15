using ShoppingList4.Blazor.Entity;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IAccountService
    {
        Task LoginAsync(User user);
    }
}