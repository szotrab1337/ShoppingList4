using MediatR;

namespace ShoppingList4.Api.Application.Entries.Commands.DeleteEntry
{
    public class DeleteEntryCommand(int entryId) : IRequest
    {
        public int Id { get; } = entryId;
    }
}
