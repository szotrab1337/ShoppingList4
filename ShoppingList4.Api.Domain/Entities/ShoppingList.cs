namespace ShoppingList4.Api.Domain.Entities
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<Entry> Entries { get; set; } = [];
    }
}
