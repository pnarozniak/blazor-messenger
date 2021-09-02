using System.Threading.Tasks;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        public Task<MessageResponseDto> AddNewMessageAsync(string idSender, NewMessageRequestDto newMessage);
    }
}
