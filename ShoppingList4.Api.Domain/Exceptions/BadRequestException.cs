namespace ShoppingList4.Api.Domain.Exceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
    }
}