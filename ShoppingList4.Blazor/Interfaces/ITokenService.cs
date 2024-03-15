namespace ShoppingList4.Blazor.Interfaces
{
    public interface ITokenService
    {
        Task<bool> Exists();
        Task<string?> Get();
        Task Remove();
        Task Set(string token);
    }
}