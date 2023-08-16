namespace ShoppingList4.Maui.Interfaces;
using Entry = ShoppingList4.Maui.Entity.Entry;

public interface IEntryService
{
    Task<List<Entry>> GetAsync(int shoppingListId);
}