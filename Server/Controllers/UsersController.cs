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
    public class UsersController : ControllerBase
    {
        private readonly ILoggedUserService _loggedUserService;
        private readonly IUsersRepository _usersRepository;

        public UsersController(
            ILoggedUserService loggedUserService,
            IUsersRepository usersRepository)
        {
            _loggedUserService = loggedUserService;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] GetDataRequestDto getDataRequest)
        {
            if (getDataRequest.Filter is null)
                return BadRequest();

            return Ok(await _usersRepository.GetUserStrangersByFilterAsync(
                _loggedUserService.Id, getDataRequest.Filter));
        }
    }
}