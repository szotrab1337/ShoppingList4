namespace ShoppingList4.Api.Models
{
    public class DeleteEntryDto
    {
        public int Id { get; set; }

        public DeleteEntryDto(int id)
        {
            Id = id;
        }
    }
}
