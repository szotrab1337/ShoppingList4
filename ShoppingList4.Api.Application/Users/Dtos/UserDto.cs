namespace ShoppingList4.Api.Application.Users.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string ApiToken { get; set; }
    }
}
