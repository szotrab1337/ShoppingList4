using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Exceptions;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Infrastructure.Common;
using ShoppingList4.Infrastructure.Extensions;

namespace ShoppingList4.Infrastructure.Repositories
{
    public class EntryRepository(
        IHttpClientFactory httpClientFactory) : IEntryRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<(bool, Entry)> Add(string? token, object content, CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            var result = await HttpClientUtilities<Entry>.Post(
                client,
                "entries",
                content,
                cancellationToken);

            if (!result.Item1 || result.Item2 is null)
            {
                throw new InvalidOperationException();
            }

            return result!;
        }

        public async Task<bool> Delete(string? token, int id, CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            return await HttpClientUtilities<bool>.Delete(
                client,
                $"entries/{id}",
                cancellationToken);
        }

        public async Task<bool> DeleteMultiple(string? token, IEnumerable<int> ids,
            CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            return await HttpClientUtilities<bool>.Delete(
                client,
                "entries/multiple",
                ids,
                cancellationToken);
        }

        public async Task<(bool, Entry)> Edit(string? token, object content,
            CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            var result = await HttpClientUtilities<Entry>.Put(
                client,
                "entries",
                content,
                cancellationToken);

            if (!result.Item1 || result.Item2 is null)
            {
                throw new InvalidOperationException();
            }

            return result!;
        }

        public async Task<Entry> Get(string? token, int id, CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            return await HttpClientUtilities<Entry>
                       .GetSingle(
                           client,
                           $"entries/{id}",
                           cancellationToken)
                   ?? throw new NotFoundException(nameof(Entry), id.ToString());
        }

        public async Task<IEnumerable<Entry>> GetByShoppingListId(string? token, int shoppingListId,
            CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            return await HttpClientUtilities<Entry>
                .GetMultiple(
                    client,
                    $"entries/shopping-list/{shoppingListId}",
                    cancellationToken);
        }
    }
}