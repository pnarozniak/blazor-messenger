using System.Collections.Generic;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Services.Interfaces
{
    public interface IAppStateService
    {
        public List<ConversationResponseDto> Conversations { get; }
        public ConversationResponseDto ActiveConversation { get; }

        public void AddConversations(List<ConversationResponseDto> newConversations);
        public void SetActiveConversation(ConversationResponseDto newActiveConversation);
    }
}
