using FluentValidation;

namespace ShoppingList4.Api.Application.ShoppingLists.Commands.EditShoppingList
{
    public class EditShoppingListCommandValidator : AbstractValidator<EditShoppingListCommand>
    {
        public EditShoppingListCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);
        }
    }
}