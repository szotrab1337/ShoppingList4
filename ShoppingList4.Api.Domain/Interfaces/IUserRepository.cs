using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> Get(int id);
        Task<User?> Get(string email);
        bool EmailExists(string email);
    }
}