namespace ShoppingList4.Api.Application.ShoppingLists.Dtos
{
    public class ShoppingListDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ItemsCount { get; set; }
        public int ItemsBoughtCount { get; set; }
    }
}