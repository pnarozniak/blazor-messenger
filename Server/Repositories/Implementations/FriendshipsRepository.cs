using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using messanger.Server.Data;
using messanger.Server.Extensions;
using messanger.Server.Helpers;
using messanger.Server.Repositories.Interfaces;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace messanger.Server.Repositories.Implementations
{
    public class FriendshipsRepository : IFriendshipsRepository
    {
        private readonly ApplicationDbContext _context;

        public FriendshipsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AreUsersInFriendshipAsync(string idUser1, string idUser2)
        {
            return await _context.Friendships
                .AnyAsync(f =>
                    f.IdUser1 == idUser1 && f.IdUser2 == idUser2 ||
                    f.IdUser2 == idUser1 && f.IdUser1 == idUser2);
        }

        public async Task<IEnumerable<UserResponseDto>> GetUserFriendsAsync(string idUser, int skip)
        {
            const int take = 10;

            return await _context.Users
                .Where(u => u.Id != idUser)
                .WhereIsFriendWith(idUser)
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

        public async Task<IEnumerable<UserResponseDto>> GetUserFriendsByFilterAsync(string idUser,
            GetFriendsRequestDto getDto)
        {
            const int take = 10;

            var firstName = FilterHelper.GetFirstPart(getDto.Filter);
            var lastName = FilterHelper.GetLastPart(getDto.Filter);

            var query = _context.Users
                .Where(u => u.FirstName.StartsWith(firstName) && u.LastName.StartsWith(lastName))
                .Where(u => u.Id != idUser)
                .WhereIsFriendWith(idUser);

            if (getDto.OnlyWithoutPrivateConversation)
                query = query.WhereHasNoPrivateConversationWith(idUser);

            return await query.Select(u => new UserResponseDto
            {
                IdUser = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName
            })
                .Skip(getDto.Skip)
                .Take(take)
                .ToListAsync();
        }
    }
}