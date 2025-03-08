using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Dtos;

namespace ShoppingList4.Maui.Interfaces
{
    public interface IShoppingListService
    {
        Task<ShoppingList> Add(AddShoppingListDto dto);
        Task<bool> Delete(int id);
        Task<IEnumerable<ShoppingList>> GetAll();
        Task<ShoppingList> Update(EditShoppingListDto dto);
    }
}