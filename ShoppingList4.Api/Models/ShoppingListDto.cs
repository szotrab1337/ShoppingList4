namespace ShoppingList4.Api.Models
{
    public class ShoppingListDto
    {
        public string Name { get; set; }

        public ShoppingListDto(string name)
        {
            Name = name;
        }
    }
}
