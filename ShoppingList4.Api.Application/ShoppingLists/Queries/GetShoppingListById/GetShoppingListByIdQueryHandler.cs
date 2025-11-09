using AutoMapper;
using MediatR;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Exceptions;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingListById
{
    public class GetShoppingListByIdQueryHandler(
        IShoppingListRepository shoppingListRepository,
        IMapper mapper) : IRequestHandler<GetShoppingListByIdQuery, ShoppingListDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;

        public async Task<ShoppingListDto> Handle(GetShoppingListByIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _shoppingListRepository.Get(request.Id)
                       ?? throw new NotFoundException(nameof(ShoppingList), request.Id.ToString());

            return _mapper.Map<ShoppingListDto>(list);
        }
    }
}