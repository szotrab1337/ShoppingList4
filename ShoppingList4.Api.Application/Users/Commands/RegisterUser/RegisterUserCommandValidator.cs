﻿using FluentValidation;
using ShoppingList4.Api.Infrastructure.Persistence;

namespace ShoppingList4.Api.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(ShoppingListDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    if (value != "bartosz_ogrodnik@outlook.com" && value != "mroz_katarzyna@outlook.com")
                    {
                        context.AddFailure("Email", "Bad email or password.");
                    }
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(z => z.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(x => x.Email == value);

                    if (emailInUse)
                    {
                        context.AddFailure("Email", "Bad email or password.");
                    }
                });
        }
    }
}
