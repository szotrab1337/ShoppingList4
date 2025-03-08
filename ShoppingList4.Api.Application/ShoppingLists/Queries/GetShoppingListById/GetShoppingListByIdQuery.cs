using MediatR;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;

namespace ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingListById
{
    public class GetShoppingListByIdQuery(int shoppingListId) : IRequest<ShoppingListDto>
    {
        public int Id { get; } = shoppingListId;
    }
}
