using System.Threading.Tasks;
using messanger.Client.Models;

namespace messanger.Client.Services.Interfaces
{
    public interface IHttpService
    {
        public Task<HttpResponseWrapper<T>> GetAsync<T>(string url, QueryParams queryParams = null);
        public Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T data);
    }
}