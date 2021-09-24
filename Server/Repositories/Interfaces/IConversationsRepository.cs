using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Responses;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IConversationsRepository
    {
        public Task<IEnumerable<ConversationResponseDto>> GetUserRecentConversationsAsync
            (string idUser, int skip);

        public Task<IEnumerable<ConversationResponseDto>> GetUserConversationsMatchingFilterAsync
            (string idUser, string filter);

        public Task<IEnumerable<MessageResponseDto>> GetUserConversationMessagesAsync
            (int idConversation, string idUser, int skip);

        public Task<IEnumerable<string>> GetConversationMembersIdsAsync
            (int idConversation);

        public Task<int?> GetPrivateConversationIdBetweenUsersAsync
            (string idUser1, string idUser2);

        public Task<GetConversationBasicInfoResponseDto> GetUserConversationBasicInfoAsync
            (int idConversation, string idUser);
    }
}