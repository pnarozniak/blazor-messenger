using System.Collections.Generic;

namespace messanger.Shared.DTOs.Responses
{
    public class ConversationResponseDto
    {
        public int IdConversation { get; set; }
        public string Name { get; set; }
        public MessageResponseDto LastMessage { get; set; }
        public IEnumerable<UserResponseDto> Members { get; set; }
    }
}
