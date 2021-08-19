using System.Threading.Tasks;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IFriendshipsRepository
    {
        public Task<bool> AreUsersInFriendshipAsync(string idUser1, string idUser2);
    }
}
