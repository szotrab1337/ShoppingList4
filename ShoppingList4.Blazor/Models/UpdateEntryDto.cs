namespace ShoppingList4.Blazor.Models
{
    public class UpdateEntryDto(string name, bool isBought)
    {
        public string Name { get; set; } = name;
        public bool IsBought { get; set; } = isBought;
    }
}
