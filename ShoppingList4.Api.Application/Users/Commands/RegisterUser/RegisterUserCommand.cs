using MediatR;

namespace ShoppingList4.Api.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}