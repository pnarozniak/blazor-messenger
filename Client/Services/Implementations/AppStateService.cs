using System;
using System.Collections.Generic;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Services.Implementations
{
    public class AppStateService : IAppStateService
    {
        public List<ConversationResponseDto> Conversations { get; } = new();

        public ConversationResponseDto ActiveConversation { get; private set; }

        public event Action OnChange;

        public void AddConversations(List<ConversationResponseDto> newConversations)
        {
            Conversations.AddRange(newConversations);
            NotifyStateChanged();
        }

        public void SetActiveConversation(ConversationResponseDto newActiveConversation)
        {
            ActiveConversation = newActiveConversation;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
