using Newtonsoft.Json;
using System.Net.Http.Headers;
using ShoppingList4.Maui.Interfaces;
using Entry = ShoppingList4.Maui.Entity.Entry;

namespace ShoppingList4.Maui.Services
{
    public class EntryService : IEntryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenService _tokenService;

        public EntryService(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _clientFactory = clientFactory;
            _tokenService = tokenService;
        }

        public async Task<List<Entry>> GetAsync(int shoppingListId)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/shoppinglist/{shoppingListId}/entries");
            var jsonEntries = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(jsonEntries))
            {
                return null;
            }

            var entries = JsonConvert.DeserializeObject<List<Entry>>(jsonEntries);
            return entries;
        }
    }
}
