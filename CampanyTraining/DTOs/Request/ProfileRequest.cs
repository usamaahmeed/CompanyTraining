namespace CompanyTraining.DTOs.Request
{
    public class ProfileRequest
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; } 

        public IFormFile? MainImgFile { get; set; }
    }
}
