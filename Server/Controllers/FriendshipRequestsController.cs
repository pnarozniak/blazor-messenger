using System;
using messanger.Server.Repositories.Interfaces;
using messanger.Server.Services.Interfaces;
using messanger.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using messanger.Server.Hubs;
using messanger.Shared.Helpers;
using messanger.Shared.DTOs.Responses;
using Microsoft.AspNetCore.SignalR;

namespace messanger.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendshipRequestsController : ControllerBase
    {
        private readonly IFriendshipRequestsRepository _friendshipRequestsRepository;
        private readonly string _idUser;

        public FriendshipRequestsController(
            IFriendshipRequestsRepository friendshipRequestsRepository,
            ILoggedUserService loggedUserService)
        {
            _friendshipRequestsRepository = friendshipRequestsRepository;
            _idUser = loggedUserService.LoggedUser.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet("received")]
        public async Task<IActionResult> GetPendingReceivedFriendshipRequests([FromQuery] string skipQuery)
        {
            int.TryParse(skipQuery, out var skip);

            var receivedFriendshipRequests = await _friendshipRequestsRepository
                .GetReceivedFriendshipRequestsAsync(_idUser, skip);

            return Ok(receivedFriendshipRequests);
        }

        [HttpGet("sent")]
        public async Task<IActionResult> GetPendingSentFriendshipRequests([FromQuery] string skipQuery)
        {
            int.TryParse(skipQuery, out var skip);

            var sentFriendshipRequests = await _friendshipRequestsRepository
                .GetSentFriendshipRequestsAsync(_idUser, skip);

            return Ok(sentFriendshipRequests);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendshipRequest([FromBody] CreateFriendshipRequestRequestDto createDto)
        {
            if (_idUser == createDto.IdReceiver)
                return BadRequest();

            var createStatus = await _friendshipRequestsRepository.CreateFriendshipRequestAsync(
                _idUser,
                createDto.IdReceiver);

            if (createStatus != CreateFriendshipRequestStatus.CREATED)
                return Conflict(new CreateFriendshipRequestResponseDto { Status = createStatus });

            return NoContent();
        }

        [HttpPost("{idReceiver}/un-sent")]
        public async Task<IActionResult> UnSentFriendshipRequest(string idReceiver)
        {
            var isDeleted = await _friendshipRequestsRepository.DeleteFriendshipRequestAsync(
                _idUser, idReceiver);

            return @isDeleted switch
            {
                true => NoContent(),
                false => NotFound()
            };
        }

        [HttpPost("{idSender}/reject")]
        public async Task<IActionResult> RejectFriendshipRequest(string idSender)
        {
            var isRejected = await _friendshipRequestsRepository.DeleteFriendshipRequestAsync(
                idSender, _idUser);

            return @isRejected switch
            {
                true => NoContent(),
                false => NotFound()
            };
        }

        [HttpPost("{idSender}/accept")]
        public async Task<IActionResult> AcceptFriendshipRequest(string idSender)
        {
            var isAccepted = await _friendshipRequestsRepository.AcceptFriendshipRequestAsync(
                idSender, _idUser);

            return @isAccepted switch
            {
                true => NoContent(),
                false => NotFound()
            };
        }
    }
}
