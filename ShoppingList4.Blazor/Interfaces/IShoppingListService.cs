using ShoppingList4.Blazor.Entities;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IShoppingListService
    {
        Task<bool> Add(string name);
        Task<bool> Delete(int id);
        Task<ShoppingList?> Get(int id);
        Task<List<ShoppingList>> GetAll();
        Task<bool> Update(ShoppingList shoppingList);
    }
}