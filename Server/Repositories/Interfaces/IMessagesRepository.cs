using System.Threading.Tasks;
using messanger.Server.Models;
using messanger.Shared.DTOs.Requests;
using messanger.Shared.DTOs.Responses;

namespace messanger.Server.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        public Task<MessageResponseDto> AddNewMessageAsync(string idSender, NewMessageRequestDto newMessage);
        public Task<Message> DeleteMessageAsync(int idMessage, string idSender);
    }
}
