using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Exceptions;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Infrastructure.Common;
using ShoppingList4.Infrastructure.Extensions;

namespace ShoppingList4.Infrastructure.Repositories
{
    public class ShoppingListRepository(
        IHttpClientFactory httpClientFactory) : IShoppingListRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<(bool, ShoppingList)> Add(string? token, object content,
            CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            var result = await HttpClientUtilities<ShoppingList>.Post(
                client,
                "shopping-lists",
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
                $"shopping-lists/{id}",
                cancellationToken);
        }

        public async Task<(bool, ShoppingList)> Edit(string? token, object content,
            CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            var result = await HttpClientUtilities<ShoppingList>.Put(
                client,
                "shopping-lists",
                content,
                cancellationToken);

            if (!result.Item1 || result.Item2 is null)
            {
                throw new InvalidOperationException();
            }

            return result!;
        }

        public async Task<ShoppingList> Get(string? token, int id, CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            return await HttpClientUtilities<ShoppingList>
                       .GetSingle(
                           client,
                           $"shopping-lists/{id}",
                           cancellationToken)
                   ?? throw new NotFoundException(nameof(ShoppingList), id.ToString());
        }

        public async Task<IEnumerable<ShoppingList>> GetAll(string? token, CancellationToken? cancellationToken = null)
        {
            using var client = _httpClientFactory.CreateClient("ShoppingList4Api").AddJwt(token);

            return await HttpClientUtilities<ShoppingList>
                .GetMultiple(client,
                    "shopping-lists/all",
                    cancellationToken);
        }
    }
}