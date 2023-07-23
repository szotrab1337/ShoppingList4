using FluentValidation;

namespace ShoppingList4.Api.Models.Validators
{
    public class ShoppingListDtoValidator : AbstractValidator<ShoppingListDto>
    {
        public ShoppingListDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);
        }
    }
}
