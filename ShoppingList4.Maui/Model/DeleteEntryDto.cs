namespace ShoppingList4.Maui.Model
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
