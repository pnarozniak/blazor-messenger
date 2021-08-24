using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<UserResponseDto> GetUserByIdAsync(string idUser);
        public Task<IEnumerable<UserResponseDto>> GetUserStrangersByFilterAsync(string idUser, string filter);
    }
}
