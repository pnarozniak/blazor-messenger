using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Services.Interfaces;

namespace messanger.Client.Services.Implementations
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private static JsonSerializerOptions DefaultJsonSerializerOptions =>
            new() { PropertyNameCaseInsensitive = true };

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url, QueryParams queryParams = null)
        {
            if (queryParams is not null) url += $"?{queryParams}";

            var responseHttp = await _httpClient.GetAsync(url);

            if (!responseHttp.IsSuccessStatusCode)
                return new HttpResponseWrapper<T>(default, false, responseHttp);

            var response = await Deserialize<T>(responseHttp, DefaultJsonSerializerOptions);
            return new HttpResponseWrapper<T>(response, true, responseHttp);
        }

        private static async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseString)
                ? default
                : JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}