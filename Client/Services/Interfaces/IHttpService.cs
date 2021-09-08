using System.Threading.Tasks;
using messanger.Client.Models;

namespace messanger.Client.Services.Interfaces
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url, QueryParams queryParams = null);
    }
}