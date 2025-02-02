using MediatR;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;

namespace ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingLists
{
    public class GetShoppingListsQuery : IRequest<IEnumerable<ShoppingListDto>>
    {
    }
}
