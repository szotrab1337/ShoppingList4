using ShoppingList4.Blazor.Dtos;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IEntryService
    {
        Task<IEnumerable<Entry>> GetShoppingListEntries(int shoppingListId);
        Task<Entry> Update(EditEntryDto dto);
        Task<bool> Delete(int id);
        Task<bool> DeleteMultiple(IEnumerable<int> ids);
        Task<Entry> Add(AddEntryDto dto);
    }
}