using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;
using ShoppingList4.Api.Infrastructure.Persistence;

namespace ShoppingList4.Api.Infrastructure.Repositories
{
    public class EntryRepository(ShoppingListDbContext dbContext) : IEntryRepository
    {
        private readonly ShoppingListDbContext _dbContext = dbContext;

        public async Task Add(Entry entry)
        {
            _dbContext.Add(entry);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _dbContext.Entries
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task Delete(IEnumerable<int> ids)
        {
            await _dbContext.Entries
                .Where(x => ids.Contains(x.Id))
                .ExecuteDeleteAsync();
        }

        public async Task<Entry?> Get(int id)
        {
            return await _dbContext.Entries
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Entry entry)
        {
            await _dbContext.Entries
                .Where(x => x.Id == entry.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.Name, entry.Name)
                    .SetProperty(z => z.IsBought, entry.IsBought)
                );
        }

        public async Task<IEnumerable<Entry>> GetByShoppingListId(int shoppingListId)
        {
            return await _dbContext.Entries
                .Where(x => x.ShoppingListId == shoppingListId)
                .ToListAsync();
        }
    }
}
