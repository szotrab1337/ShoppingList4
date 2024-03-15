using Newtonsoft.Json;
using ShoppingList4.Blazor.Entity;
using ShoppingList4.Blazor.Interfaces;
using System.Text;

namespace ShoppingList4.Blazor.Services
{
    public class AccountService(IHttpClientFactory clientFactory, ITokenService tokenService) : IAccountService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly ITokenService _tokenService = tokenService;

        private async Task<string> GetToken(User user)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/account/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task LoginAsync(User user)
        {
            var token = await GetToken(user);

            if (string.IsNullOrEmpty(token))
            {
                await _tokenService.Remove();
                return;
            }

            await _tokenService.Set(token);
        }
    }
}
