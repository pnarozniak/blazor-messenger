using System.Net.Http;
using System.Text;
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

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<TR>> PostWithJsonResponseAsync<T, TR>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, stringContent);

            if (!responseHttp.IsSuccessStatusCode)
                return new HttpResponseWrapper<TR>(default, false, responseHttp);

            var response = await Deserialize<TR>(responseHttp, DefaultJsonSerializerOptions);
            return new HttpResponseWrapper<TR>(response, false, responseHttp);
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