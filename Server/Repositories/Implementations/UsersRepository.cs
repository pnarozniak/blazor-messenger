using System.Threading.Tasks;
using messanger.Server.Data;
using messanger.Server.Repositories.Interfaces;
using messanger.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace messanger.Server.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;
        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(string idUser)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == idUser);
            return new UserResponseDto
            {
                IdUser = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
