using Microsoft.AspNetCore.Identity;

namespace CompanyTraining.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }

    }
}
