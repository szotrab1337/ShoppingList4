using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;
using ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingListById;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.EditShoppingList
{
    public class EditShoppingListCommandHandler(
        IShoppingListRepository shoppingListRepository,
        IMapper mapper,
        ILogger<EditShoppingListCommandHandler> logger,
        IMediator mediator) : IRequestHandler<EditShoppingListCommand, ShoppingListDto>
    {
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EditShoppingListCommandHandler> _logger = logger;
        private readonly IMediator _mediator = mediator;

        public async Task<ShoppingListDto> Handle(EditShoppingListCommand request, CancellationToken cancellationToken)
        {
            var list = _mapper.Map<ShoppingList>(request);
            await _shoppingListRepository.Update(list);

            _logger.LogInformation("Updated shopping list {@List}.",
                list);

            return await _mediator.Send(new GetShoppingListByIdQuery(list.Id), cancellationToken);
        }
    }
}
