using Microsoft.AspNetCore.Identity;

namespace CampanyTraining.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }

    }
}
