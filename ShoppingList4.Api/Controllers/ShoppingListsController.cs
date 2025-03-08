using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingList4.Api.Application.ShoppingLists.Commands.AddShoppingList;
using ShoppingList4.Api.Application.ShoppingLists.Commands.DeleteShoppingList;
using ShoppingList4.Api.Application.ShoppingLists.Commands.EditShoppingList;
using ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingListById;
using ShoppingList4.Api.Application.ShoppingLists.Queries.GetShoppingLists;

namespace ShoppingList4.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/shopping-lists")]
    [Authorize]
    public class ShoppingListsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var lists = await _mediator.Send(new GetShoppingListsQuery());

            return Ok(lists);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddShoppingListCommand command)
        {
            var list = await _mediator.Send(command);

            return Created($"/api/v1/shopping-lists/{list.Id}", list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var list = await _mediator.Send(new GetShoppingListByIdQuery(id));

            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteShoppingListCommand(id));

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Update(EditShoppingListCommand command)
        {
            var list = await _mediator.Send(command);

            return Ok(list);
        }
    }
}
