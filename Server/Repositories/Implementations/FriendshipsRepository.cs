using messanger.Server.Repositories.Interfaces;
using messanger.Server.Data;
using System.Threading.Tasks;
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
                                (f.IdUser1 == idUser1 && f.IdUser2 == idUser2) ||
                                (f.IdUser2 == idUser1 && f.IdUser1 == idUser2));
        }
    }
}
