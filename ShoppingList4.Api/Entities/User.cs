using System.Data;
#pragma warning disable CS8618

namespace ShoppingList4.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
