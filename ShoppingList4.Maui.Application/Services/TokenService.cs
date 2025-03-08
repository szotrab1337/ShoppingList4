using ShoppingList4.Maui.Interfaces;
using Microsoft.Maui.Storage;

namespace ShoppingList4.Maui.Application.Services
{
    public class TokenService : ITokenService
    {
        public async Task<bool> ExistsAsync()
        {
            var token = await SecureStorage.GetAsync("Bearer");

            return !string.IsNullOrEmpty(token);
        }

        public void Remove()
        {
            SecureStorage.Remove("Bearer");
        }

        public async Task SetAsync(string token)
        {
            await SecureStorage.SetAsync("Bearer", token);
        }

        public async Task<string> GetAsync()
        {
            return await SecureStorage.GetAsync("Bearer");
        }
    }
}
