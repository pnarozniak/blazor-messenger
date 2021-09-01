using System.Threading.Tasks;
using messanger.Server.Hubs;
using messanger.Server.Repositories.Interfaces;
using messanger.Server.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace messanger.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationsRepository _conversationsRepository;
        private readonly ILoggedUserService _loggedUserService;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _notificationsHubContext;

        public ConversationsController(
            ILoggedUserService loggedUserService,
            IConversationsRepository conversationsRepository,
            IHubContext<NotificationsHub, INotificationsHub> notificationsHubContext)
        {
            _loggedUserService = loggedUserService;
            _conversationsRepository = conversationsRepository;
            _notificationsHubContext = notificationsHubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations(
            [FromQuery] GetDataRequestDto getDataRequest)
        {
            if (getDataRequest.Filter is not null)
                return Ok(await _conversationsRepository
                    .GetUserConversationsMatchingFilterAsync(_loggedUserService.Id, getDataRequest.Filter));

            return Ok(await _conversationsRepository
                .GetUserRecentConversationsAsync(_loggedUserService.Id, getDataRequest.Skip));
        }

        [HttpGet("{idConversation:int}/messages")]
        public async Task<IActionResult> GetConversationMessages(
            [FromRoute] int idConversation, [FromQuery] int skip)
        {
            var messages = await _conversationsRepository.GetUserConversationMessagesAsync(
                idConversation, _loggedUserService.Id, skip);

            if (messages is null)
                return BadRequest();

            return Ok(messages);
        }

        [HttpPost("{idConversation:int}/send-message")]
        public async Task<IActionResult> SendMessageInConversation(
            [FromRoute] int idConversation, [FromBody] NewMessageRequestDto newMessage)
        {
            var addedMessage = await _conversationsRepository.AddNewMessageToUserConversationAsync
                (idConversation, _loggedUserService.Id, newMessage);

            if (addedMessage is null)
                return NotFound();

            var conversationMembersIds = await _conversationsRepository.GetConversationMembersIdsAsync(idConversation);
            await _notificationsHubContext.Clients.Users(conversationMembersIds)
                .NewMessage(idConversation, addedMessage);

            return NoContent();
        }
    }
}