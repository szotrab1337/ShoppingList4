using MediatR;
using ShoppingList4.Api.Application.Entries.Dtos;

namespace ShoppingList4.Api.Application.Entries.Commands.AddEntry
{
    public class AddEntryCommand : IRequest<EntryDto>
    {
        public required string Name { get; set; }
        public int ShoppingListId { get; set; }
    }
}
