using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Client.Models;
using messanger.Client.Repositories.Interfaces;
using messanger.Client.Services.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Repositories.Implementations
{
    public class FriendshipsRepository : IFriendshipsRepository
    {
        private const string ApiBaseUrl = "api/friendships";
        private readonly IHttpService _httpService;

        public FriendshipsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetFriendsAsync(GetFriendsRequestDto getDto = default)
        {
            getDto ??= new GetFriendsRequestDto();

            var queryParams = new QueryParams();
            queryParams.Add(nameof(getDto.Skip), getDto.Skip);
            queryParams.Add(nameof(getDto.OnlyWithoutPrivateConversation), getDto.OnlyWithoutPrivateConversation);
            queryParams.Add(nameof(getDto.Filter), getDto.Filter);

            var response = await _httpService.GetAsync<IEnumerable<UserResponseDto>>
                ($"{ApiBaseUrl}", queryParams);
            return response.Response;
        }
    }
}