using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;
using ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingListById;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.AddShoppingList
{
    public class AddShoppingListCommandHandler(
        IShoppingListRepository shoppingListRepository,
        IMapper mapper,
        ILogger<AddShoppingListCommandHandler> logger,
        IMediator mediator) : IRequestHandler<AddShoppingListCommand, ShoppingListDto>
    {
        private readonly ILogger<AddShoppingListCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;

        public async Task<ShoppingListDto> Handle(AddShoppingListCommand request, CancellationToken cancellationToken)
        {
            var list = _mapper.Map<ShoppingList>(request);
            await _shoppingListRepository.Add(list);

            _logger.LogInformation("Shopping list {@List} been added to database.",
                list);

            return await _mediator.Send(new GetShoppingListByIdQuery(list.Id), cancellationToken);
        }
    }
}