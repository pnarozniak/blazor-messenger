using System;
using System.Collections.Generic;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Services.Interfaces
{
    public interface IAppStateService
    {
        public List<ConversationResponseDto> Conversations { get; }

        public int? ActiveConversationId { get; }

        public event Action OnChange;

        public void AddConversations(List<ConversationResponseDto> newConversations);
        public void SetActiveConversation(int newActiveConversationId);
    }
}
