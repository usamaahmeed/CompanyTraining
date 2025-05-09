using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class RegisterDTO
    {
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        [Required]
        public IFormFile MainImgFile { get; set; } = null!;
        [Required]
        public IFormFile CoverImgFile { get; set; } = null!;
        [Required]
        public int PackageId { get; set; }

        public string Role { get; set; }="Company";
    }
}
