namespace ShoppingList4.Application.Dtos
{
    public class AddEntryDto
    {
        public int ShoppingListId { get; set; }
        public required string Name { get; set; }
    }
}