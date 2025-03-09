using ShoppingList4.Blazor.Dtos;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Blazor.Interfaces
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingList>> GetAll();
        Task<ShoppingList> Add(AddShoppingListDto dto);
        Task<bool> Delete(int id);
        Task<ShoppingList> Update(EditShoppingListDto dto);
    }
}