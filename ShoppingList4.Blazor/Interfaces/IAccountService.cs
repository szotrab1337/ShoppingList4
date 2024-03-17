using ShoppingList4.Blazor.Entities;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IAccountService
    {
        Task LoginAsync(User user);
    }
}