using Newtonsoft.Json;
using ShoppingList4.Blazor.Entities;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Blazor.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ShoppingList4.Blazor.Services
{
    public class EntryService(IHttpClientFactory clientFactory, ITokenService tokenService) : IEntryService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<List<Entry>> Get(int shoppingListId)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.Get();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/shoppinglist/{shoppingListId}/entries");
            var jsonEntries = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(jsonEntries))
            {
                return [];
            }

            return JsonConvert.DeserializeObject<List<Entry>>(jsonEntries)!;
        }

        public async Task<bool> Update(Entry entry)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.Get();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var entryDto = new UpdateEntryDto(entry.Name, entry.IsBought);
            var content = new StringContent(JsonConvert.SerializeObject(entryDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/entry/{entry.Id}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.Get();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/entry/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMultiple(List<int> ids)
        {
            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.Get();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var entries = new List<DeleteEntryDto>();
            ids.ForEach(x => entries.Add(new DeleteEntryDto(x)));

            var content = new StringContent(JsonConvert.SerializeObject(entries), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/entry/deleteMultiple", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Add(string name, int shoppingListId)
        {
            var entry = new EntryDto(name, shoppingListId);

            using var client = _clientFactory.CreateClient("ShoppingList4");

            var token = await _tokenService.Get();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/entry", content);

            return response.IsSuccessStatusCode;
        }
    }
}
