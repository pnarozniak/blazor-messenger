using System.ComponentModel.DataAnnotations;

namespace messanger.Shared.DTOs.Requests
{
    public class GetFriendsRequestDto
    {
        [Range(0, int.MaxValue)] public int Skip { get; set; }

        public bool OnlyWithoutPrivateConversation { get; set; }

        [RegularExpression(".*\\S.*")] public string Filter { get; set; }
    }
}