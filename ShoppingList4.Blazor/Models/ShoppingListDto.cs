namespace ShoppingList4.Blazor.Models
{
    public class ShoppingListDto(string name)
    {
        public string Name { get; set; } = name;
    }
}
