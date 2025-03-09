namespace ShoppingList4.Blazor.Dtos
{
    public class AddEntryDto
    {
        public int ShoppingListId { get; set; }
        public required string Name { get; set; }
    }
}
