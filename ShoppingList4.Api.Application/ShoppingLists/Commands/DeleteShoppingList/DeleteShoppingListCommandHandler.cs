using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.DeleteShoppingList
{
    public class DeleteShoppingListCommandHandler(
        IShoppingListRepository shoppingListRepository,
        ILogger<DeleteShoppingListCommandHandler> logger) : IRequestHandler<DeleteShoppingListCommand>
    {
        private readonly ILogger<DeleteShoppingListCommandHandler> _logger = logger;
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;

        public async Task Handle(DeleteShoppingListCommand request, CancellationToken cancellationToken)
        {
            await _shoppingListRepository.Delete(request.Id);

            _logger.LogInformation("Deleted shopping list {ShoppingListId}.", request.Id);
        }
    }
}