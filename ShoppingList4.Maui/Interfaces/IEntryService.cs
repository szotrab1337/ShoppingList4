using ShoppingList4.Maui.Dtos;

namespace ShoppingList4.Maui.Interfaces
{
    public interface IEntryService
    {
        Task<Domain.Entities.Entry> Add(AddEntryDto dto);
        Task<bool> Delete(int id);
        Task<bool> DeleteMultiple(IEnumerable<int> ids);
        Task<IEnumerable<Domain.Entities.Entry>> GetShoppingListEntries(int shoppingListId);
        Task<Domain.Entities.Entry> Update(EditEntryDto dto);
    }
}