using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Components;

namespace messanger.Client.Shared.SideBar
{
    public partial class ConversationsList
    {
        [Parameter] public IEnumerable<ConversationResponseDto> Conversations { get; set; }

        [Parameter] public EventCallback<ConversationResponseDto> AfterConversationClicked { get; set; }

        [Parameter] public int? ActiveConversationId { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        private async Task ConversationClick(ConversationResponseDto conversation)
        {
            NavigationManager.NavigateTo($"conversation/{conversation.IdConversation}");

            if (AfterConversationClicked.HasDelegate)
                await AfterConversationClicked.InvokeAsync();
        }

        public void Refresh()
        {
            StateHasChanged();
        }
    }
}