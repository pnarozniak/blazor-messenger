using System.Linq;
using messanger.Server.Models;

namespace messanger.Server.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<User> WhereIsFriendWith(this IQueryable<User> srcUsersQueryable, string idUser)
        {
            return srcUsersQueryable
                .Where(u => u.FriendshipsWhereIsUser1.Any(f1 => f1.IdUser2 == idUser)
                            || u.FriendshipsWhereIsUser1.Any(f2 => f2.IdUser1 == idUser));
        }

        public static IQueryable<User> WhereIsNotFriendWith(this IQueryable<User> srcUsersQueryable, string idUser)
        {
            return srcUsersQueryable
                .Where(u => u.FriendshipsWhereIsUser1.All(f1 => f1.IdUser2 != idUser)
                            && u.FriendshipsWhereIsUser1.All(f2 => f2.IdUser1 != idUser));
        }
    }
}


