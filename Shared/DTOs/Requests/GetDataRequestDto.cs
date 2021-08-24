using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace messanger.Shared.DTOs.Requests
{
    public class GetDataRequestDto
    {
        [Range(0, int.MaxValue)]
        public int Skip { get; set; }

        [RegularExpression(".*\\S.*")]
        public string Filter { get; set; }
    }
}
