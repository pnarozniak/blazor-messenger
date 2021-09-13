using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Client.Repositories.Interfaces;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Components;

namespace messanger.Client.Shared.SideBar
{
    public partial class UsersList
    {
        [Parameter] public IEnumerable<UserResponseDto> Users { get; set; }

        [Parameter] public EventCallback<UserResponseDto> AfterUserClicked { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IConversationsRepository ConversationsRepository { get; set; }

        private async Task UserClick(UserResponseDto user)
        {
            var idConversation = await ConversationsRepository
                .GetPrivateConversationIdWithUserAsync(user.IdUser);


            NavigationManager.NavigateTo(idConversation is null
                ? $"conversation/new/{user.IdUser}"
                : $"conversation/{idConversation}");

            if (AfterUserClicked.HasDelegate)
                await AfterUserClicked.InvokeAsync();
        }
    }
}