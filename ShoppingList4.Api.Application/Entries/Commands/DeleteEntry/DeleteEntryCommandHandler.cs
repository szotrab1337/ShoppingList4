using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Entries.Commands.DeleteEntry
{
    public class DeleteEntryCommandHandler(
        IEntryRepository entryRepository,
        ILogger<DeleteEntryCommandHandler> logger) : IRequestHandler<DeleteEntryCommand>
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly ILogger<DeleteEntryCommandHandler> _logger = logger;

        public async Task Handle(DeleteEntryCommand request, CancellationToken cancellationToken)
        {
            await _entryRepository.Delete(request.Id);

            _logger.LogInformation("Deleted entry {EntryId}.", request.Id);
        }
    }
}
