using AutoMapper;
using MediatR;
using ShoppingList4.Api.Application.Entries.Dtos;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Entries.Queries.GetEntriesByShoppingListId
{
    public class GetEntriesByShoppingListIdQueryHandler(
        IEntryRepository entryRepository,
        IMapper mapper) : IRequestHandler<GetEntriesByShoppingListIdQuery, IEnumerable<EntryDto>>
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<EntryDto>> Handle(GetEntriesByShoppingListIdQuery request,
            CancellationToken cancellationToken)
        {
            var entries = await _entryRepository.GetByShoppingListId(request.ShoppingListId);

            return _mapper.Map<IEnumerable<EntryDto>>(entries);
        }
    }
}