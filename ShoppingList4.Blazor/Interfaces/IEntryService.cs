using ShoppingList4.Blazor.Entities;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IEntryService
    {
        Task<bool> Add(string name, int shoppingListId);
        Task<bool> Delete(int id);
        Task<bool> DeleteMultiple(List<int> ids);
        Task<List<Entry>> Get(int shoppingListId);
        Task<bool> Update(Entry entry);
    }
}