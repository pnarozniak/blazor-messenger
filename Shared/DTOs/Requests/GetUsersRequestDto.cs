using System.ComponentModel.DataAnnotations;

namespace messanger.Shared.DTOs.Requests
{
    public class GetUsersRequestDto
    {
        [Required]
        [RegularExpression(".*\\S.*")]
        public string Filter { get; set; }
    }
}