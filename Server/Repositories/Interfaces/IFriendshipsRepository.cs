using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IFriendshipsRepository
    {
        public Task<bool> AreUsersInFriendshipAsync(string idUser1, string idUser2);
        public Task<IEnumerable<UserResponseDto>> GetUserFriendsAsync(string idUser, int skip);

        public Task<IEnumerable<UserResponseDto>> GetUserFriendsByFilterAsync(string idUser,
            GetFriendsRequestDto getDto);
    }
}