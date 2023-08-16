namespace ShoppingList4.Maui.Model
{
    public class UpdateEntryDto
    {
        public string Name { get; set; }
        public bool IsBought { get; set; }

        public UpdateEntryDto(string name, bool isBought)
        {
            Name = name;
            IsBought = isBought;
        }
    }
}
