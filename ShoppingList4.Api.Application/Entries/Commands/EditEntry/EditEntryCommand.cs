using MediatR;
using ShoppingList4.Api.Application.Entries.Dtos;

namespace ShoppingList4.Api.Application.Entries.Commands.EditEntry
{
    public class EditEntryCommand : IRequest<EntryDto>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsBought { get; set; }
    }
}
