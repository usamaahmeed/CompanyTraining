using System.ComponentModel.DataAnnotations;

namespace CompanyTraining.DTOs.Request
{
    public class AddUserRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
