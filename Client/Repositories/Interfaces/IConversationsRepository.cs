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
    }
}