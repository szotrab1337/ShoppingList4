using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Entries.Commands.DeleteEntries
{
    public class DeleteEntriesCommandHandler(
        IEntryRepository entryRepository,
        ILogger<DeleteEntriesCommandHandler> logger) : IRequestHandler<DeleteEntriesCommand>
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly ILogger<DeleteEntriesCommandHandler> _logger = logger;

        public async Task Handle(DeleteEntriesCommand request, CancellationToken cancellationToken)
        {
            await _entryRepository.Delete(request.Ids);

            _logger.LogInformation("Deleted entries {@EntryIds}.", request.Ids);
        }
    }
}