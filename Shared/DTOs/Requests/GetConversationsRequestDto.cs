using System.ComponentModel.DataAnnotations;

namespace messanger.Shared.DTOs.Requests
{
    public class GetConversationsRequestDto
    {
        [Range(0, int.MaxValue)] public int Skip { get; set; }

        [RegularExpression(".*\\S.*")] public string Filter { get; set; }
    }
}