using System.Threading.Tasks;
using messanger.Shared.DTOs.Requests;

namespace messanger.Client.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        public Task<bool> CreateMessageAsync(NewMessageRequestDto newMessageRequest);
    }
}