using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Models
{
    public class MessageWrapper
    {
        public MessageResponseDto Message { get; set; }
        public bool IsSender { get; set; }
        public bool IsRoot { get; set; }
    }
}
