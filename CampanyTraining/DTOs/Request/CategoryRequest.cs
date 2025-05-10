using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; }=string.Empty;

    }
}
