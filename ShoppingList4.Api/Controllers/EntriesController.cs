using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingList4.Api.Application.Entries.Commands.AddEntry;
using ShoppingList4.Api.Application.Entries.Commands.DeleteEntries;
using ShoppingList4.Api.Application.Entries.Commands.DeleteEntry;
using ShoppingList4.Api.Application.Entries.Commands.EditEntry;
using ShoppingList4.Api.Application.Entries.Dtos;
using ShoppingList4.Api.Application.Entries.Queries.GetEntriesByShoppingListId;
using ShoppingList4.Api.Application.Entries.Queries.GetEntryById;

namespace ShoppingList4.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize]
    public class EntriesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> Add(AddEntryCommand command)
        {
            var entry = await _mediator.Send(command);

            return Created($"/api/v1/entries/{entry.Id}", entry);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteEntryCommand(id));

            return NoContent();
        }

        [HttpDelete("multiple")]
        public async Task<ActionResult> DeleteMultiple(IEnumerable<int> ids)
        {
            await _mediator.Send(new DeleteEntriesCommand(ids));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntryDto>> Get(int id)
        {
            var entry = await _mediator.Send(new GetEntryByIdQuery(id));

            return Ok(entry);
        }

        [HttpGet("shopping-list/{shoppingListId}")]
        public async Task<ActionResult> GetShoppingListEntries(int shoppingListId)
        {
            var entries = await _mediator.Send(new GetEntriesByShoppingListIdQuery(shoppingListId));

            return Ok(entries);
        }

        [HttpPut]
        public async Task<ActionResult> Update(EditEntryCommand command)
        {
            var entry = await _mediator.Send(command);

            return Ok(entry);
        }
    }
}