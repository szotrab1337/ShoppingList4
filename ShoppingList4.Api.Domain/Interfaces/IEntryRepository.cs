using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Domain.Interfaces
{
    public interface IEntryRepository
    {
        Task Add(Entry entry);
        Task Delete(IEnumerable<int> ids);
        Task Delete(int id);
        Task<Entry?> Get(int id);
        Task<IEnumerable<Entry>> GetByShoppingListId(int shoppingListId);
        Task Update(Entry entry);
    }
}