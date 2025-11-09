using AutoMapper;
using MediatR;
using ShoppingList4.Api.Application.ShoppingLists.Dtos;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingLists
{
    public class GetShoppingListsQueryHandler(
        IShoppingListRepository shoppingListRepository,
        IMapper mapper) : IRequestHandler<GetShoppingListsQuery, IEnumerable<ShoppingListDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;

        public async Task<IEnumerable<ShoppingListDto>> Handle(GetShoppingListsQuery request,
            CancellationToken cancellationToken)
        {
            var lists = await _shoppingListRepository.GetAll();

            return _mapper.Map<IEnumerable<ShoppingListDto>>(lists);
        }
    }
}