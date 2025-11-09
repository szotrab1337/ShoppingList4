using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;
using ShoppingList4.Api.Infrastructure.Persistence;

namespace ShoppingList4.Api.Infrastructure.Repositories
{
    public class UserRepository(ShoppingListDbContext dbContext) : IUserRepository
    {
        private readonly ShoppingListDbContext _dbContext = dbContext;

        public async Task Add(User user)
        {
            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();
        }

        public bool EmailExists(string email)
        {
            return _dbContext.Users.Any(x => x.Email == email);
        }

        public async Task<User?> Get(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> Get(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}