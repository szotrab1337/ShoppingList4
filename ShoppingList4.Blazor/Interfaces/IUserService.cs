using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsCurrentUser();
        Task RemoveCurrentUser();
        Task SetCurrentUser(User user);
        Task<User?> GetCurrentUser();
    }
}