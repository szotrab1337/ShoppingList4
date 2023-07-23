#pragma warning disable CS8618
namespace ShoppingList4.Api.Models
{
    public class CreateEntryDto
    {
        public string Name { get; set; }
        public int ShoppingListId { get; set; }
    }
}
