using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Repositories.Implementations
{
    public class ConversationsRepository : IConversationsRepository
    {
        private const string ApiBaseUrl = "api/conversations";
        private readonly IHttpService _httpService;

        public ConversationsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<ConversationResponseDto>> GetConversationsAsync(
            GetConversationsRequestDto getDto = default)
        {
            getDto ??= new GetConversationsRequestDto();

            var queryParams = new QueryParams();
            queryParams.Add(nameof(getDto.Skip), getDto.Skip);
            queryParams.Add(nameof(getDto.Filter), getDto.Filter);

            var response = await _httpService.GetAsync<IEnumerable<ConversationResponseDto>>
                ($"{ApiBaseUrl}", queryParams);
            return response.Response;
        }

        public async Task<int?> GetPrivateConversationIdWithUserAsync(string idUser)
        {
            var response = await _httpService.GetAsync<int?>
                ($"{ApiBaseUrl}/users/{idUser}");

            return response.HttpResponseMessage.StatusCode == HttpStatusCode.OK ? response.Response : null;
        }

        public async Task<IEnumerable<MessageResponseDto>> GetConversationMessagesAsync(int idConversation,
            int skip = default)
        {
            var queryParams = new QueryParams();
            queryParams.Add(nameof(skip), skip);

            var response = await _httpService.GetAsync<IEnumerable<MessageResponseDto>>
                ($"{ApiBaseUrl}/{idConversation}/messages", queryParams);
            return response.Response;
        }

        public async Task<GetConversationBasicInfoResponseDto> GetBasicConversationInfoAsync(int idConversation)
        {
            var response = await _httpService.GetAsync<GetConversationBasicInfoResponseDto>
                ($"{ApiBaseUrl}/{idConversation}");
            return response.Success ? response.Response : null;
        }

        public async Task<bool> SendMessageInConversationAsync(int idConversation, NewMessageRequestDto newMessageRequest)
        {
            var response = await _httpService.PostAsync($"{ApiBaseUrl}/{idConversation}/messages", newMessageRequest);
            return response.Success;
        }
    }
}