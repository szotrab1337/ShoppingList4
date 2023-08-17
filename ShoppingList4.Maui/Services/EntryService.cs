using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Model;
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

        public async Task<bool> UpdateAsync(Entry entry)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //TODO: dodać automapper
            var entryDto = new UpdateEntryDto(entry.Name, entry.IsBought);
            var content = new StringContent(JsonConvert.SerializeObject(entryDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/entry/{entry.Id}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/entry/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMultipleAsync(List<int> ids)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var entries = new List<DeleteEntryDto>();
            ids.ForEach(x => entries.Add(new DeleteEntryDto(x)));

            var content = new StringContent(JsonConvert.SerializeObject(entries), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/entry/deleteMultiple", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(string name, int shoppingListId)
        {
            var entry = new EntryDto(name, shoppingListId);

            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.GetAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/entry", content);

            return response.IsSuccessStatusCode;
        }
    }
}
