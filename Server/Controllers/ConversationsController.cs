using System.Threading.Tasks;
using messanger.Server.Repositories.Interfaces;
using messanger.Server.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace messanger.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationsRepository _conversationsRepository;
        private readonly ILoggedUserService _loggedUserService;

        public ConversationsController(
            ILoggedUserService loggedUserService,
            IConversationsRepository conversationsRepository)
        {
            _loggedUserService = loggedUserService;
            _conversationsRepository = conversationsRepository;
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
    }
}