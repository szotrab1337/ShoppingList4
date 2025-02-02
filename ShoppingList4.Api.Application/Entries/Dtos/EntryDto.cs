namespace ShoppingList4.Api.Application.Entries.Dtos
{
    public class EntryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsBought { get; set; }
        public int ShoppingListId { get; set; }
    }
}
