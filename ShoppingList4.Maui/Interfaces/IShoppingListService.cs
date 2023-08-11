using ShoppingList4.Maui.Entity;

namespace ShoppingList4.Maui.Interfaces;

public interface IShoppingListService
{
    Task<List<ShoppingList>> GetAll();
    Task<bool> Delete(int id);
    Task<bool> Add(string name);
}