using System.Threading.Tasks;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Components;

namespace messanger.Client.Pages.Conversation
{
    public partial class Conversation
    {
        private GetConversationBasicInfoResponseDto _basicConversationInfo;

        [Parameter] public int IdConversation { get; set; }

        [Inject] public IAppStateService AppStateService { get; set; }

        [Inject] public IMessagesRepository MessagesRepository { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            AppStateService.SetActiveConversation(IdConversation);
            _basicConversationInfo = await ConversationsRepository
                .GetBasicConversationInfoAsync(IdConversation);
            if (_basicConversationInfo is null)
            {
                AppStateService.SetActiveConversation(-1);
                NavigationManager.NavigateTo("/");
            }
        }

        private async Task SendMessage(string message)
        {
            await MessagesRepository.CreateMessageAsync(new NewMessageRequestDto()
            {
                IdConversation = IdConversation,
                Content = message
            });
        }
    }
}
