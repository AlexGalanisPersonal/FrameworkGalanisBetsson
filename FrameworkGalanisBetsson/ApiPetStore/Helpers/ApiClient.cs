using System.Text.Json;
using Microsoft.Playwright;

namespace SauceDemoUiBetsson.ApiPetStore.Helpers;

public class ApiClient(IAPIRequestContext requestContext, string baseUrl)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public async Task<T?> SendRequest<T>(string endpoint, string method, object? data = null)
    {
        var url = $"{baseUrl}{endpoint}";

        // Create options with headers and data
        var fetchOptions = new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Accept", "application/json" }
            },
            Method = method
        };

        if (data != null)
        {
            fetchOptions.DataObject = data;
        }

        var response = await requestContext.FetchAsync(url, fetchOptions);
        var content = await response.TextAsync();

        if (!response.Ok)
        {
            throw new HttpRequestException($"API request failed with status {response.Status}: {content}");
        }

        return !string.IsNullOrEmpty(content) 
            ? JsonSerializer.Deserialize<T>(content, _jsonOptions) 
            : default;
    }

    public async Task<T?> Get<T>(string endpoint) => 
        await SendRequest<T>(endpoint, "GET");

    public async Task<T?> Post<T>(string endpoint, object data) => 
        await SendRequest<T>(endpoint, "POST", data);

    public async Task<T?> Put<T>(string endpoint, object data) => 
        await SendRequest<T>(endpoint, "PUT", data);

    public async Task<T?> Delete<T>(string endpoint) => 
        await SendRequest<T>(endpoint, "DELETE");
}