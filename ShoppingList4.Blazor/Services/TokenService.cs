using Blazored.LocalStorage;
using ShoppingList4.Blazor.Interfaces;

namespace ShoppingList4.Blazor.Services
{
    public class TokenService(ILocalStorageService localStorage) : ITokenService
    {
        private readonly ILocalStorageService _localStorage = localStorage;

        public async Task<bool> Exists()
        {
            var token = await Get();

            return !string.IsNullOrEmpty(token);
        }

        public async Task Remove()
        {
            await _localStorage.RemoveItemAsync("Token");
        }

        public async Task Set(string token)
        {
            await _localStorage.SetItemAsync("Token", token);
        }

        public async Task<string?> Get()
        {
            return await _localStorage.GetItemAsync<string>("Token");
        }
    }
}
