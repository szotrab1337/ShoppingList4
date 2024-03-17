namespace ShoppingList4.Blazor.Models
{
    public class DeleteEntryDto(int id)
    {
        public int Id { get; set; } = id;
    }
}
