namespace ShoppingList4.Application.Dtos
{
    public class EditEntryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsBought { get; set; }
    }
}