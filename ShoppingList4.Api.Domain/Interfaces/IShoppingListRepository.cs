using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Domain.Interfaces
{
    public interface IShoppingListRepository
    {
        Task Add(ShoppingList list);
        Task Delete(int id);
        Task<ShoppingList?> Get(int id);
        Task<IEnumerable<ShoppingList>> GetAll();
        Task Update(ShoppingList list);
    }
}