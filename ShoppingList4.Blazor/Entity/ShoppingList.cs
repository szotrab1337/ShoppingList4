namespace ShoppingList4.Blazor.Entity
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public int EntriesNotBought => Entries.Count(x => !x.IsBought);

        public virtual List<Entry> Entries { get; set; } = new();
    }
}
