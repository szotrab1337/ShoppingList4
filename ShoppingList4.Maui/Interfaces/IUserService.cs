using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Maui.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsCurrentUser();
        void RemoveCurrentUser();
        Task SetCurrentUser(User user);
        Task<User?> GetCurrentUser();
    }
}