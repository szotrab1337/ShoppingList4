using ShoppingList4.Maui.Entity;

namespace ShoppingList4.Maui.Interfaces;

public interface IShoppingListService
{
    Task<List<ShoppingList>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
    Task<bool> AddAsync(string name);
    Task<bool> UpdateAsync(ShoppingList shoppingList);
}