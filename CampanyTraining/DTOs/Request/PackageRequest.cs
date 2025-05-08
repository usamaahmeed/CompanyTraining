using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class PackageRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int DurationDay { get; set; }

    }
}
