using FluentValidation;

namespace ShoppingList4.Api.Models.Validators
{
    public class UpdateEntryDtoValidator : AbstractValidator<UpdateEntryDto>
    {
        public UpdateEntryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);
        }
    }
}
