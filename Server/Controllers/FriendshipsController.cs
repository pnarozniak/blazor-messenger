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
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipsRepository _friendshipsRepository;
        private readonly ILoggedUserService _loggedUserService;

        public FriendshipsController(
            ILoggedUserService loggedUserService,
            IFriendshipsRepository friendshipsRepository)
        {
            _loggedUserService = loggedUserService;
            _friendshipsRepository = friendshipsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends(
            [FromQuery] GetFriendsRequestDto getDto)
        {
            if (getDto.Filter is null)
                return Ok(await _friendshipsRepository.GetUserFriendsAsync(
                    _loggedUserService.Id, getDto.Skip));

            return Ok(await _friendshipsRepository.GetUserFriendsByFilterAsync(
                _loggedUserService.Id, getDto));
        }
    }
}