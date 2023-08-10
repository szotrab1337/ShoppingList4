namespace ShoppingList4.Maui.Entity
{
    public class Entry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsBought { get; set; }

        public int ShoppingListId { get; set; }
    }
}
