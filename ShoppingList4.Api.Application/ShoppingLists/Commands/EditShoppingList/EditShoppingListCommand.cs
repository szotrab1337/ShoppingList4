using MediatR;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.EditShoppingList
{
    public class EditShoppingListCommand : IRequest<ShoppingListDto>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
