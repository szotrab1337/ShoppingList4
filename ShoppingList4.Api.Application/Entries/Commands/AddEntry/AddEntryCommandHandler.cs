using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Application.Entries.Dtos;
using ShoppingList4.Api.Application.Entries.Queries.GetEntryById;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Entries.Commands.AddEntry
{
    public class AddEntryCommandHandler(
        IEntryRepository entryRepository,
        IMapper mapper,
        ILogger<AddEntryCommandHandler> logger,
        IMediator mediator) : IRequestHandler<AddEntryCommand, EntryDto>
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly ILogger<AddEntryCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        public async Task<EntryDto> Handle(AddEntryCommand request, CancellationToken cancellationToken)
        {
            var entry = _mapper.Map<Entry>(request);
            await _entryRepository.Add(entry);

            _logger.LogInformation("Entry {@Entry} has been added to database.",
                entry);

            return await _mediator.Send(new GetEntryByIdQuery(entry.Id), cancellationToken);
        }
    }
}