using FluentValidation;
using ShoppingList4.Api.Entities;

namespace ShoppingList4.Api.Models.Validators
{
    public class CreateEntryDtoValidator : AbstractValidator<CreateEntryDto>
    {
        public CreateEntryDtoValidator(ShoppingListDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(35);

            RuleFor(x => x.ShoppingListId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.ShoppingListId)
                .Custom((value, context) =>
                {
                    var shoppingListInDb = dbContext.ShoppingLists.Any(x => x.Id == value);

                    if (!shoppingListInDb)
                    {
                        context.AddFailure("ShoppingListId", "Shopping list is not present in db");
                    }
                });
        }
    }
}
