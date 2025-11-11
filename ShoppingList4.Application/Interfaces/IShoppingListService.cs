using ShoppingList4.Application.Dtos;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Application.Interfaces
{
    public interface IShoppingListService
    {
        event EventHandler<ShoppingList>? ShoppingListAdded;
        event EventHandler<int>? ShoppingListDeleted;
        event EventHandler<ShoppingList>? ShoppingListUpdated;
        Task Add(AddShoppingListDto dto);
        Task Delete(int id);
        Task<IEnumerable<ShoppingList>> GetAll();
        Task Update(EditShoppingListDto dto);
    }
}