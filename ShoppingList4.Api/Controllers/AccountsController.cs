using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingList4.Api.Application.Users.Commands.Login;
using ShoppingList4.Api.Application.Users.Commands.RegisterUser;

namespace ShoppingList4.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class AccountsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginCommand command)
        {
            var user = await _mediator.Send(command);

            return Ok(user);
        }
    }
}
