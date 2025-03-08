using System.Net.Http.Headers;

namespace ShoppingList4.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddJwt(this HttpClient httpClient, string? apiToken)
        {
            if (string.IsNullOrEmpty(apiToken))
            {
                throw new InvalidOperationException();
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

            return httpClient;
        }
    }
}