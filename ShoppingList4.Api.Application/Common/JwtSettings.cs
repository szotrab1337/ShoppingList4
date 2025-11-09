namespace ShoppingList4.Api.Application.Common
{
    public class JwtSettings
    {
        public required string SecurityKey { get; set; }
        public int ExpireDays { get; set; }
        public required string Issuer { get; set; }
    }
}