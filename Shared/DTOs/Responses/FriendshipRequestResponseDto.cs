using System;

namespace messanger.Shared.DTOs
{
    public class FriendshipRequestResponseDto
    {
        public UserResponseDto User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
