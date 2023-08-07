#nullable enable
using Newtonsoft.Json;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using System.Text;

namespace ShoppingList4.Maui.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenService _tokenService;

        public AccountService(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _clientFactory = clientFactory;
            _tokenService = tokenService;
        }

        private async Task<string> GetToken(User user)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/account/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task LoginAsync(User user)
        {
            var token = await GetToken(user);

            if (string.IsNullOrEmpty(token))
            {
                _tokenService.Remove();
                return;
            }

            await _tokenService.SetAsync(token);
        }
    }
}
