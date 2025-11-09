using FluentValidation;

namespace ShoppingList4.Api.Application.Entries.Commands.AddEntry
{
    public class AddEntryCommandValidator : AbstractValidator<AddEntryCommand>
    {
        public AddEntryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);

            RuleFor(x => x.ShoppingListId)
                .NotEmpty();
        }
    }
}