using MediatR;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.DeleteShoppingList
{
    public class DeleteShoppingListCommand(int shoppingListId) : IRequest
    {
        public int Id { get; } = shoppingListId;
    }
}
