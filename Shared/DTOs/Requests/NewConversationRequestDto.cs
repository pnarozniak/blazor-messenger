using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace messanger.Shared.DTOs.Requests
{
    public class NewConversationRequestDto
    {
        [Required]
        public NewMessageRequestDto InitialMessage { get; set; }

        [Required, MinLength(1)]
        public IEnumerable<string> ParticipantsIds { get; set; }
    }
}
