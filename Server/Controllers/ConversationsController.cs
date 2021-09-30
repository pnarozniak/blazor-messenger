using System;
using System.Linq;
using System.Threading.Tasks;
using messanger.Server.Hubs;
using messanger.Server.Repositories.Interfaces;
using messanger.Server.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;
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
        private readonly IMessagesRepository _messagesRepository;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _notificationsHubContext;

        public ConversationsController(
            ILoggedUserService loggedUserService,
            IConversationsRepository conversationsRepository,
            IMessagesRepository messagesRepository,
            IHubContext<NotificationsHub, INotificationsHub> notificationsHubContext)
        {
            _loggedUserService = loggedUserService;
            _conversationsRepository = conversationsRepository;
            _messagesRepository = messagesRepository;
            _notificationsHubContext = notificationsHubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations(
            [FromQuery] GetConversationsRequestDto getDto)
        {
            if (getDto.Filter is not null)
                return Ok(await _conversationsRepository
                    .GetUserConversationsMatchingFilterAsync(_loggedUserService.Id, getDto.Filter));

            return Ok(await _conversationsRepository
                .GetUserRecentConversationsAsync(_loggedUserService.Id, getDto.Skip));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewConversationWithInitialMessage(
                [FromBody] NewConversationRequestDto newConversation)
        {
            (int idConversation, MessageResponseDto message)? createConversationResponse;
            if (newConversation.ParticipantsIds.Count() == 1)
            {
                createConversationResponse = await _conversationsRepository.CreatePrivateConversationAsync(
                    _loggedUserService.Id, newConversation.ParticipantsIds.First(),
                    newConversation.InitialMessage);
            }
            else
            {
                createConversationResponse = await _conversationsRepository.CreateGroupConversationAsync(
                    _loggedUserService.Id, newConversation.ParticipantsIds,
                    newConversation.InitialMessage);
            }

            if (createConversationResponse is null)
                return Conflict();

            await _notificationsHubContext.Clients
                .Users(newConversation.ParticipantsIds.Append(_loggedUserService.Id))
                .NewMessage(createConversationResponse.Value.idConversation, createConversationResponse.Value.message);

            return Ok(createConversationResponse.Value.idConversation);
        }

        [HttpGet("{idConversation:int}")]
        public async Task<IActionResult> GetConversationBasicInfo(
            [FromRoute] int idConversation)
        {
            var basicInfo = await _conversationsRepository.GetUserConversationBasicInfoAsync(
                idConversation, _loggedUserService.Id);

            if (basicInfo is null)
                return BadRequest();

            return Ok(basicInfo);
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

        [HttpPost("{idConversation:int}/messages")]
        public async Task<IActionResult> CreateMessageInConversation(
            [FromRoute] int idConversation, [FromBody] NewMessageRequestDto newMessage)
        {
            var addedMessage = await _messagesRepository
                .AddNewMessageAsync(_loggedUserService.Id, idConversation, newMessage);

            if (addedMessage is null)
                return BadRequest();

            var conversationMembersIds = await _conversationsRepository
                .GetConversationMembersIdsAsync(idConversation);

            await _notificationsHubContext.Clients.Users(conversationMembersIds)
                .NewMessage(idConversation, addedMessage);

            return NoContent();
        }

        [HttpGet("users/{idUser}")]
        public async Task<IActionResult> GetPrivateConversationIdWithUser(
            [FromRoute] string idUser)
        {
            var idConversation = await _conversationsRepository.GetPrivateConversationIdBetweenUsersAsync(
                _loggedUserService.Id, idUser);

            return idConversation is null ? NoContent() : Ok(idConversation);
        }
    }
}