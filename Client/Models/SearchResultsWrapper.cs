using System.Collections.Generic;
using messanger.Shared.DTOs.Responses;

namespace messanger.Client.Models
{
    public class SearchResultsWrapper
    {
        public IEnumerable<ConversationResponseDto> Conversations { get; set; }
        public IEnumerable<UserResponseDto> Friends { get; set; }
        public IEnumerable<UserResponseDto> MorePeople { get; set; }
    }
}
