using ShoppingList4.Maui.Entity;

namespace ShoppingList4.Maui.Entity
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public int EntriesNotBought => Entries.Count(x => !x.IsBought);

        public virtual List<Entry> Entries { get; set; } = new();
    }
}
