using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private const string ApiBaseUrl = "api/users";
        private readonly IHttpService _httpService;

        public UsersRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersAsync(GetUsersRequestDto getDto)
        {
            var queryParams = new QueryParams();
            queryParams.Add(nameof(getDto.Filter), getDto.Filter);

            var response = await _httpService.GetAsync<IEnumerable<UserResponseDto>>
                ($"{ApiBaseUrl}", queryParams);
            return response.Response;
        }
    }
}