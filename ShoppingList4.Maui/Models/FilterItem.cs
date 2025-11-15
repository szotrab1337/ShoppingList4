namespace ShoppingList4.Maui.Models
{
    public class FilterItem(string displayText, string filter)
    {
        public string DisplayText { get; } = displayText;
        public string Filter { get; } = filter;
    }
}