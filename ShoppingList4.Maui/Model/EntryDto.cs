namespace ShoppingList4.Maui.Model
{
    public class EntryDto
    {
        public EntryDto(string name, int shoppingListId)
        {
            Name = name;
            ShoppingListId = shoppingListId;
        }

        public string Name { get; set; }
        public int ShoppingListId { get; set; }
    }
}
