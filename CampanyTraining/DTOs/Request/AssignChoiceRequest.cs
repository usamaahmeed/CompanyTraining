using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class AssignChoiceRequest
    {
        [Required]
        [MinLength(2, ErrorMessage = "At least 2 choices required.")]
        public IEnumerable<ChoiceDto> Choices { get; set; } = null!;
    }
}
