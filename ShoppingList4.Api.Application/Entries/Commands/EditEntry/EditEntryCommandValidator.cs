using FluentValidation;

namespace ShoppingList4.Api.Application.Entries.Commands.EditEntry
{
    public class EditEntryCommandValidator : AbstractValidator<EditEntryCommand>
    {
        public EditEntryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);
        }
    }
}
