using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Domain.Interfaces
{
    public interface IShoppingListRepository
    {
        Task<(bool, ShoppingList)> Add(string? token, object content, CancellationToken? cancellationToken = null);
        Task<bool> Delete(string? token, int id, CancellationToken? cancellationToken = null);
        Task<(bool, ShoppingList)> Edit(string? token, object content, CancellationToken? cancellationToken = null);
        Task<ShoppingList> Get(string? token, int id, CancellationToken? cancellationToken = null);
        Task<IEnumerable<ShoppingList>> GetAll(string? token, CancellationToken? cancellationToken = null);
    }
}