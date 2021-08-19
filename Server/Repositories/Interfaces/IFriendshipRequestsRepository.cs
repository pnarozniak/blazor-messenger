using System.Collections.Generic;
using System.Threading.Tasks;
using messanger.Shared.Helpers;
using messanger.Shared.DTOs;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IFriendshipRequestsRepository
    {
        public Task<IEnumerable<FriendshipRequestResponseDto>> GetSentFriendshipRequestsAsync(string idUser, int skip);
        public Task<IEnumerable<FriendshipRequestResponseDto>> GetReceivedFriendshipRequestsAsync(string idUser, int skip);
        public Task<CreateFriendshipRequestStatus> CreateFriendshipRequestAsync(string idSender, string idReceiver);
        public Task<bool> DeleteFriendshipRequestAsync(string idSender, string idReceiver);
        public Task<bool> AcceptFriendshipRequestAsync(string idSender, string idReceiver);
    }
}


