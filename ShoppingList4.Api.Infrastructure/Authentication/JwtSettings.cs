namespace ShoppingList4.Api.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public required string SecurityKey { get; set; }
        public int ExpireDays { get; set; }
        public required string Issuer { get; set; }
    }
}