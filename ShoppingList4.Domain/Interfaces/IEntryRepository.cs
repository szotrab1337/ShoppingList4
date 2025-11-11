using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Domain.Interfaces
{
    public interface IEntryRepository
    {
        Task<(bool, Entry)> Add(string? token, object content, CancellationToken? cancellationToken = null);
        Task<bool> Delete(string? token, int id, CancellationToken? cancellationToken = null);
        Task<bool> DeleteMultiple(string? token, IEnumerable<int> ids, CancellationToken? cancellationToken = null);
        Task<(bool, Entry)> Edit(string? token, object content, CancellationToken? cancellationToken = null);
        Task<Entry> Get(string? token, int id, CancellationToken? cancellationToken = null);

        Task<IEnumerable<Entry>> GetByShoppingListId(string? token, int shoppingListId,
            CancellationToken? cancellationToken = null);
    }
}