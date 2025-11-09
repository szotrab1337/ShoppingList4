using MediatR;

namespace ShoppingList4.Api.Application.Entries.Commands.DeleteEntries
{
    public class DeleteEntriesCommand(IEnumerable<int> entryIds) : IRequest
    {
        public IEnumerable<int> Ids { get; } = entryIds;
    }
}