using System.ComponentModel.DataAnnotations;

namespace messanger.Shared.DTOs.Requests
{
    public class NewMessageRequestDto
    {
        [Required]
        public string Content { get; set; }
    }
}
