using System;
using System.Text;
using System.Text.Json;

namespace ShoppingList4.Infrastructure.Common
{
    public static class HttpClientUtilities<T>
    {
        public static async Task<IEnumerable<T>> GetMultiple(
            HttpClient client,
            string requestUri,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;
            var response = await client.GetAsync(requestUri, token);
            var jsonResponse = await response.Content.ReadAsStringAsync(token);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Error occurred during executing {requestUri}. Code: {response.StatusCode}");
            }

            return JsonSerializer.Deserialize<IEnumerable<T>>(jsonResponse, JsonSerializerUtilities.BasicOptions) ??
                   [];
        }

        public static async Task<T?> GetSingle(
            HttpClient client,
            string requestUri,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;
            var response = await client.GetAsync(requestUri, token);
            var jsonResponse = await response.Content.ReadAsStringAsync(token);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Error occurred during executing {requestUri}. Code: {response.StatusCode}");
            }

            return JsonSerializer.Deserialize<T>(jsonResponse, JsonSerializerUtilities.BasicOptions);
        }

        public static async Task<(bool, T?)> Post(
            HttpClient client,
            string requestUri,
            object? content,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;

            var stringContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                requestUri,
                stringContent,
                token);

            var jsonResponse = await response.Content.ReadAsStringAsync(token);
            var objectResponse = string.IsNullOrEmpty(jsonResponse)
                ? default
                : JsonSerializer.Deserialize<T>(jsonResponse, JsonSerializerUtilities.BasicOptions);

            return (response.IsSuccessStatusCode, objectResponse);
        }

        public static async Task<bool> Delete(
            HttpClient client,
            string requestUri,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;

            var response = await client.DeleteAsync(
                requestUri,
                token);

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> Delete(
            HttpClient client,
            string requestUri,
            object content,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;

            var stringContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = stringContent };

            var response = await client.SendAsync(request, token);

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> PostWithoutResponse(
            HttpClient client,
            string requestUri,
            object? content,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;

            var stringContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                requestUri,
                stringContent,
                token);

            return response.IsSuccessStatusCode;
        }

        public static async Task<(bool, T?)> Put(
            HttpClient client,
            string requestUri,
            object? content,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;

            var stringContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync(
                requestUri,
                stringContent,
                token);

            var jsonResponse = await response.Content.ReadAsStringAsync(token);
            var objectResponse = string.IsNullOrEmpty(jsonResponse)
                ? default
                : JsonSerializer.Deserialize<T>(jsonResponse, JsonSerializerUtilities.BasicOptions);

            return (response.IsSuccessStatusCode, objectResponse);
        }

        public static async Task<(bool, T?)> Patch(
            HttpClient client,
            string requestUri,
            object? content,
            CancellationToken? cancellationToken)
        {
            var token = cancellationToken ?? CancellationToken.None;

            var stringContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var response = await client.PatchAsync(
                requestUri,
                stringContent,
                token);

            var jsonResponse = await response.Content.ReadAsStringAsync(token);
            var objectResponse = string.IsNullOrEmpty(jsonResponse)
                ? default
                : JsonSerializer.Deserialize<T>(jsonResponse, JsonSerializerUtilities.BasicOptions);

            return (response.IsSuccessStatusCode, objectResponse);
        }
    }
}