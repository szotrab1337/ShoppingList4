using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsCurrentUser();
        Task<User?> GetCurrentUser();
        void RemoveCurrentUser();
        Task SetCurrentUser(User user);
    }
}