using MediatR;
using ShoppingList4.Api.Application.Entries.Dtos;

namespace ShoppingList4.Api.Application.Entries.Queries.GetEntryById
{
    public class GetEntryByIdQuery(int entryId) : IRequest<EntryDto>
    {
        public int Id { get; } = entryId;
    }
}
