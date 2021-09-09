using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Repositories.Interfaces
{
    public interface IFriendshipsRepository
    {
        public Task<IEnumerable<UserResponseDto>> GetFriendsAsync(GetFriendsRequestDto getDto = default);
    }
}