using System.Text.Json;

namespace ShoppingList4.Infrastructure.Common
{
    public static class JsonSerializerUtilities
    {
        public static readonly JsonSerializerOptions BasicOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
