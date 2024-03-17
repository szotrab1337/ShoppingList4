namespace ShoppingList4.Blazor.Models
{
    public class EntryDto(string name, int shoppingListId)
    {
        public string Name { get; set; } = name;
        public int ShoppingListId { get; set; } = shoppingListId;
    }
}
