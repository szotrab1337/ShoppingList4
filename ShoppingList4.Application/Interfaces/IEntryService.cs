using ShoppingList4.Application.Dtos;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Application.Interfaces
{
    public interface IEntryService
    {
        event EventHandler<Entry>? EntryAdded;
        event EventHandler<int>? EntryDeleted;
        event EventHandler<Entry>? EntryUpdated;
        Task Add(AddEntryDto dto);
        Task Delete(int id);
        Task DeleteMultiple(List<int> ids);
        Task<IEnumerable<Entry>> GetShoppingListEntries(int shoppingListId);
        Task Update(EditEntryDto dto);
    }
}