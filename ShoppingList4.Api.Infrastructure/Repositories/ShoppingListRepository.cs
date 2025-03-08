using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;
using ShoppingList4.Api.Infrastructure.Persistence;

namespace ShoppingList4.Api.Infrastructure.Repositories
{
    public class ShoppingListRepository(ShoppingListDbContext dbContext) : IShoppingListRepository
    {
        private readonly ShoppingListDbContext _dbContext = dbContext;

        public async Task<ShoppingList?> Get(int id)
        {
            return await _dbContext.ShoppingLists
                .Include(x => x.Entries)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ShoppingList>> GetAll()
        {
            return await _dbContext.ShoppingLists
                .Include(x => x.Entries)
                .ToListAsync();
        }

        public async Task Add(ShoppingList list)
        {
            _dbContext.Add(list);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.ShoppingLists
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task Update(ShoppingList list)
        {
            await _dbContext.ShoppingLists
                .Where(x => x.Id == list.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.Name, list.Name)
                );
        }
    }
}
