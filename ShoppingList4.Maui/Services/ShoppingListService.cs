using System.Net.Http.Headers;
using Newtonsoft.Json;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenService _tokenService;

        public ShoppingListService(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _clientFactory = clientFactory;
            _tokenService = tokenService;
        }

        public async Task<List<ShoppingList>> GetAll()
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/shoppinglist");
            var rawShoppingLists = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(rawShoppingLists))
            {
                return null;
            }

            var shoppingLists = JsonConvert.DeserializeObject<List<ShoppingList>>(rawShoppingLists);
            return shoppingLists;
        }

        public async Task<bool> Delete(int id)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/shoppinglist/{id}");
            
            return response.IsSuccessStatusCode;
        }
    }
}
