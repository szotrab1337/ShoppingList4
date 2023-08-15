using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ShoppingList4.Maui.Entity;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Model;

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

        public async Task<bool> Add(string name)
        {
            var shoppingList = new ShoppingListDto(name);

            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(shoppingList), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/shoppinglist", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/shoppinglist/{id}");
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(ShoppingList shoppingList)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(shoppingList), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/shoppingList/{shoppingList.Id}", content);

            return response.IsSuccessStatusCode;
        }
    }
}
