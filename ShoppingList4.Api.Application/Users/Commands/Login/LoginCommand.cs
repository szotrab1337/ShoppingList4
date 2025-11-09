using MediatR;
using ShoppingList4.Api.Application.Users.Dtos;

namespace ShoppingList4.Api.Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<UserDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}