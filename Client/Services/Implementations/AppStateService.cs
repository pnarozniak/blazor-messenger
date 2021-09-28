using System;
using System.Collections.Generic;
using System.Linq;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Services.Implementations
{
    public class AppStateService : IAppStateService
    {
        public List<ConversationResponseDto> Conversations { get; } = new();

        public int? ActiveConversationId { get; private set; }

        public event Action OnChange;

        public void AddConversations(List<ConversationResponseDto> newConversations)
        {
            var selectedConversations = newConversations
                .Where(newConversation =>
                    Conversations.All(c => c.IdConversation != newConversation.IdConversation))
                .ToList();

            if (!selectedConversations.Any())
                return;

            Conversations.AddRange(selectedConversations);
            NotifyStateChanged();
        }

        public void AddConversationAt0(ConversationResponseDto newConversation)
        {
            Conversations.Insert(0, newConversation);
            NotifyStateChanged();
        }

        public void SetActiveConversation(int newActiveConversationId)
        {
            ActiveConversationId = newActiveConversationId;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
