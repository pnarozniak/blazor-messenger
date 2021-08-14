using System.ComponentModel.DataAnnotations;

namespace messanger.Shared.DTOs
{
    public class CreateFriendshipRequestRequestDto
    {
        [Required]
        [StringLength(36, MinimumLength = 36)]
        public string IdReceiver { get; set; }
    }
}
