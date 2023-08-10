namespace ShoppingList4.Maui.Interfaces;

public interface ITokenService
{
    Task<bool> ExistsAsync();
    void Remove();
    Task SetAsync(string token);
    Task<string> GetAsync();
}