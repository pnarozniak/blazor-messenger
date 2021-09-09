using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<UserResponseDto>> GetUsersAsync(GetUsersRequestDto getDto);
    }
}