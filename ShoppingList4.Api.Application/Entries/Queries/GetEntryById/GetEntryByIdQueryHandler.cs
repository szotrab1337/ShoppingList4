using AutoMapper;
using MediatR;
using ShoppingList4.Api.Application.Entries.Dtos;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Exceptions;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Entries.Queries.GetEntryById
{
    public class GetEntryByIdQueryHandler(
        IEntryRepository entryRepository,
        IMapper mapper) : IRequestHandler<GetEntryByIdQuery, EntryDto>
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EntryDto> Handle(GetEntryByIdQuery request, CancellationToken cancellationToken)
        {
            var entry = await _entryRepository.Get(request.Id)
                        ?? throw new NotFoundException(nameof(Entry), request.Id.ToString());

            return _mapper.Map<EntryDto>(entry);
        }
    }
}