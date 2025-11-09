using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Application.Entries.Dtos;
using ShoppingList4.Api.Application.Entries.Queries.GetEntryById;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Entries.Commands.EditEntry
{
    public class EditEntryCommandHandler(
        IEntryRepository entryRepository,
        IMapper mapper,
        ILogger<EditEntryCommandHandler> logger,
        IMediator mediator) : IRequestHandler<EditEntryCommand, EntryDto>
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly ILogger<EditEntryCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        public async Task<EntryDto> Handle(EditEntryCommand request, CancellationToken cancellationToken)
        {
            var entry = _mapper.Map<Entry>(request);
            await _entryRepository.Update(entry);

            _logger.LogInformation("Updated entry {@Entry}.",
                entry);

            return await _mediator.Send(new GetEntryByIdQuery(entry.Id), cancellationToken);
        }
    }
}