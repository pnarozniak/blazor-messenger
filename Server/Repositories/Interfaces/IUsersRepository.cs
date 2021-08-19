using System.Threading.Tasks;
using messanger.Shared.DTOs;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<UserResponseDto> GetUserByIdAsync(string idUser);
    }
}
