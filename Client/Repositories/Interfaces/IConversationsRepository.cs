using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Repositories.Interfaces
{
    public interface IConversationsRepository
    {
        public Task<IEnumerable<ConversationResponseDto>> GetConversationsAsync(
            GetConversationsRequestDto getDto = default);

        public Task<int?> GetPrivateConversationIdWithUserAsync(string idUser);

        public Task<IEnumerable<MessageResponseDto>> GetConversationMessagesAsync(
            int idConversation, int skip = default);

        public Task<GetConversationBasicInfoResponseDto> GetBasicConversationInfoAsync(int idConversation);

        public Task<bool> SendMessageInConversationAsync(int idConversation, NewMessageRequestDto newMessageRequest);
    }
}