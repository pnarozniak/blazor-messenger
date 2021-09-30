using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Components;

namespace messanger.Client.Pages.Conversation.New
{
    public partial class NewPrivateConversation
    {
        private bool _isPending;
        private NewPrivateConversationParameters _parameters;

        [Inject] public ISessionStorageService SessionStorageService { get; set; }
        [Inject] public IConversationsRepository ConversationsRepository { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IAppStateService AppStateService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!await SessionStorageService.ContainKeyAsync("NewPrivateConversationParameters"))
            {
                NavigationManager.NavigateTo("/", true);
                return;
            }

            _parameters =
                await SessionStorageService.GetItemAsync<NewPrivateConversationParameters>(
                    "NewPrivateConversationParameters");

            await SessionStorageService.RemoveItemAsync("NewPrivateConversationParameters");
        }

        public async Task CreateConversationBySendingInitialMessage(string message)
        {
            if (_isPending) return;

            _isPending = true;
            var createdConversationId = await ConversationsRepository.CreateConversationAsync(
                new NewConversationRequestDto
                {
                    InitialMessage = new NewMessageRequestDto
                    {
                        Content = message
                    },
                    ParticipantsIds = new List<string> { _parameters.IdUser }
                });

            if (createdConversationId is not null)
            {
                NavigationManager.NavigateTo($"/conversation/{createdConversationId}");
                AppStateService.AddConversationAt0(
                    new ConversationResponseDto
                    {
                        IdConversation = (int)createdConversationId,
                        Name = _parameters.ConversationName,
                        LastMessage = new MessageResponseDto
                        {
                            Content = message,
                            CreatedAt = DateTime.UtcNow
                        }
                    }
                );
            }
            else
            {
                NavigationManager.NavigateTo("/", true);
            }

            _isPending = false;
        }
    }
}