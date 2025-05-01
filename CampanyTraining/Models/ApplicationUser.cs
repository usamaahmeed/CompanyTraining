using Microsoft.AspNetCore.Identity;

namespace CompanyTraining.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }

        public string MainImg { get; set; } = string.Empty;

        public string CoverImg {  get; set; }=string .Empty;
    }
}
