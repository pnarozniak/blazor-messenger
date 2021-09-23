using System.Threading.Tasks;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;

namespace messanger.Client.Repositories.Implementations
{
    public class MessagesRepository : IMessagesRepository
    {
        private const string ApiBaseUrl = "/api/messages";
        private readonly IHttpService _httpService;

        public MessagesRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<bool> CreateMessageAsync(NewMessageRequestDto newMessageRequest)
        {
            var response = await _httpService.PostAsync($"{ApiBaseUrl}", newMessageRequest);
            return response.Success;
        }
    }
}