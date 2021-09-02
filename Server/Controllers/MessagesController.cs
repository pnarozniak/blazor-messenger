using System;
using System.Threading.Tasks;
using messanger.Server.Hubs;
using messanger.Server.Repositories.Interfaces;
using messanger.Server.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace messanger.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILoggedUserService _loggedUserService;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IConversationsRepository _conversationsRepository;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _notificationsHubContext;

        public MessagesController(
            ILoggedUserService loggedUserService,
            IMessagesRepository messagesRepository,
            IConversationsRepository conversationsRepository,
            IHubContext<NotificationsHub, INotificationsHub> notificationsHubContext
            )
        {
            _loggedUserService = loggedUserService;
            _messagesRepository = messagesRepository;
            _conversationsRepository = conversationsRepository;
            _notificationsHubContext = notificationsHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(
            [FromBody] NewMessageRequestDto newMessage)
        {
            var addedMessage = await _messagesRepository.AddNewMessageAsync
                (_loggedUserService.Id, newMessage);

            if (addedMessage is null)
                return NotFound();

            var conversationMembersIds = await _conversationsRepository
                .GetConversationMembersIdsAsync((int)newMessage.IdConversation);

            await _notificationsHubContext.Clients.Users(conversationMembersIds)
                .NewMessage((int)newMessage.IdConversation, addedMessage);

            return NoContent();
        }

        [HttpDelete("{idMessage:int}")]
        public async Task<IActionResult> DeleteMessage(
            [FromRoute] int idMessage)
        {
            var deletedMessage = await _messagesRepository.DeleteMessageAsync
                (idMessage, _loggedUserService.Id);

            if (deletedMessage is null)
                return NotFound();

            var conversationMembersIds = await _conversationsRepository
                .GetConversationMembersIdsAsync(deletedMessage.IdConversation);

            await _notificationsHubContext.Clients.Users(conversationMembersIds)
                .MessageDeleted(idMessage, deletedMessage.IdConversation, (DateTime)deletedMessage.DeletedAt);

            return NoContent();
        }
    }
}
