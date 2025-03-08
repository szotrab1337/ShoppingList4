using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Infrastructure.Common;
using System.Text;
using System.Text.Json;

namespace ShoppingList4.Infrastructure.Repositories
{
    public class UsersRepository(
        IHttpClientFactory httpClientFactory) : IUsersRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<User?> Login(string email, string password, CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api");

            var token = cancellationToken ?? CancellationToken.None;

            var content = new StringContent(
                JsonSerializer.Serialize(new { email, password }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("accounts/login", content, token);
            var jsonResponse = await response.Content.ReadAsStringAsync(token);

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(jsonResponse))
            {
                return null;
            }

            return JsonSerializer.Deserialize<User>(jsonResponse, JsonSerializerUtilities.BasicOptions);
        }
    }
}