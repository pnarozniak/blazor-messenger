using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace messanger.Client.Shared.Conversation
{
    public partial class MessagesList
    {
        private int _idConversation;
        private List<MessageWrapper> _messages = new();
        private string _loggedUserId;
        private ElementReference _listRef;
        private InfiniteScroll<MessageWrapper> _infiniteScrollRef;

        [Parameter] public int IdConversation { get; set; }

        [Parameter] public string ConversationName { get; set; }

        [Parameter] public string ConversationAvatar { get; set; }

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _loggedUserId = authState?.User?.FindFirst(c => c.Type == "sub")?.Value;
        }

        protected override void OnParametersSet()
        {
            if (_idConversation == IdConversation)
                return;

            _idConversation = IdConversation;
            _infiniteScrollRef?.ForceRefresh();
            _messages.Clear();
        }

        private async Task<bool> FetchMessages()
        {
            var messages = (await ConversationsRepository
                .GetConversationMessagesAsync(IdConversation, _messages.Count)).ToList();

            PrepareForDisplay(messages);
            return messages.Any();
        }

        private void PrepareForDisplay(List<MessageResponseDto> newMessages)
        {
            MessageResponseDto prevMessage = null;
            foreach (var newMessage in newMessages.Where(newMessage => _messages.All(m => m.Message.IdMessage != newMessage.IdMessage)))
            {
                _messages.Add(new MessageWrapper()
                {
                    Message = newMessage,
                    IsSender = newMessage.Sender.IdUser == _loggedUserId,
                    IsRoot = newMessage.Sender.IdUser != prevMessage?.Sender?.IdUser
                });

                prevMessage = newMessage;
            }
        }
    }
}
