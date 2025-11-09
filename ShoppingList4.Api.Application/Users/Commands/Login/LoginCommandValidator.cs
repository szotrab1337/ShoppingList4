using FluentValidation;

namespace ShoppingList4.Api.Application.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}