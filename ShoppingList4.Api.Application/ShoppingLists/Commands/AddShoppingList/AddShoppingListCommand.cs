using MediatR;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.AddShoppingList
{
    public class AddShoppingListCommand : IRequest<ShoppingListDto>
    {
        public required string Name { get; set; }
    }
}