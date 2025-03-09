namespace ShoppingList4.Blazor.Dtos
{
    public class EditEntryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsBought { get; set; }
    }
}
