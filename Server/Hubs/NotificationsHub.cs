using System;
using System.Threading.Tasks;
using messanger.Shared.DTOs;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace messanger.Server.Hubs
{
    public interface INotificationsHub
    {
        public Task NewFriendshipRequest(UserResponseDto sender);
        public Task NewMessage(int idConversation, MessageResponseDto message);
        public Task MessageDeleted(int idMessage, int idConversation, DateTime messageDeletedAt);
    }

    [Authorize]
    public class NotificationsHub : Hub<INotificationsHub>
    {

    }
}
