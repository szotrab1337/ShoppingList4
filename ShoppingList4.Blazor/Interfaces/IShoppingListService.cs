using ShoppingList4.Blazor.Entity;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IShoppingListService
    {
        Task<bool> Add(string name);
        Task<bool> Delete(int id);
        Task<List<ShoppingList>> GetAll();
        Task<bool> Update(ShoppingList shoppingList);
    }
}