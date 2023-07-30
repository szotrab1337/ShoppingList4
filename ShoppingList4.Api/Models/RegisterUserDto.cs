#pragma warning disable CS8618
namespace ShoppingList4.Api.Models
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
