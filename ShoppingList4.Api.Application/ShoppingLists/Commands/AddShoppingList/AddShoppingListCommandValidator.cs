using FluentValidation;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.AddShoppingList
{
    public class AddShoppingListCommandValidator : AbstractValidator<AddShoppingListCommand>
    {
        public AddShoppingListCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);
        }
    }
}
