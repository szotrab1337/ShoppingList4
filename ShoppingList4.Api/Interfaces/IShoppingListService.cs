using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Interfaces;

public interface IShoppingListService
{
    IEnumerable<ShoppingList> GetAll();
    int Create(ShoppingListDto dto);
    ShoppingList GetById(int id);
    void Delete(int id);
}