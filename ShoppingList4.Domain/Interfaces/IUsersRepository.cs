using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> Login(string email, string password, CancellationToken? cancellationToken = null);
    }
}