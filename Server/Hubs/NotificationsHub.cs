using System.Threading.Tasks;
using messanger.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace messanger.Server.Hubs
{
    public interface INotificationsHub
    {
        public Task NewFriendshipRequest(UserResponseDto sender);
    }

    [Authorize]
    public class NotificationsHub : Hub<INotificationsHub>
    {

    }
}
