using System;

namespace messanger.Shared.DTOs.Responses
{
    public class MessageResponseDto
    {
        public int IdMessage { get; set; }
        public UserResponseDto Sender { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
