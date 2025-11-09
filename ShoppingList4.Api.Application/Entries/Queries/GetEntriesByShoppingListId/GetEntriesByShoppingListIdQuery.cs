using MediatR;
using ShoppingList4.Api.Application.Entries.Dtos;

namespace ShoppingList4.Api.Application.Entries.Queries.GetEntriesByShoppingListId
{
    public class GetEntriesByShoppingListIdQuery(int shoppingListId) : IRequest<IEnumerable<EntryDto>>
    {
        public int ShoppingListId { get; } = shoppingListId;
    }
}