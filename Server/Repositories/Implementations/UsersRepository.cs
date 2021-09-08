using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using messanger.Server.Data;
using messanger.Server.Extensions;
using messanger.Server.Helpers;
using messanger.Server.Repositories.Interfaces;
using messanger.Shared.DTOs.Responses;
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

        public async Task<IEnumerable<UserResponseDto>> GetUserStrangersByFilterAsync(string idUser, string filter)
        {
            const int take = 10;

            var firstName = FilterHelper.GetFirstPart(filter);
            var lastName = FilterHelper.GetLastPart(filter);

            return await _context.Users
                .Where(u => u.FirstName.StartsWith(firstName) && u.LastName.StartsWith(lastName))
                .Where(u => u.Id != idUser)
                .WhereIsNotFriendWith(idUser)
                .WhereHasNoPrivateConversationWith(idUser)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new UserResponseDto
                {
                    IdUser = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .Take(take)
                .ToListAsync();
        }
    }
}