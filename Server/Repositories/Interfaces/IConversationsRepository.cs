using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Responses;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IConversationsRepository
    {
        public Task<IEnumerable<ConversationResponseDto>> GetUserRecentConversationsAsync(string idUser, int skip);
        public Task<IEnumerable<ConversationResponseDto>> GetUserConversationsMatchingFilterAsync(string idUser, string filter);
    }
}
