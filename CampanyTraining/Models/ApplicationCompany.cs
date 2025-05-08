using Microsoft.AspNetCore.Identity;

namespace CompanyTraining.Models
{
    public class ApplicationCompany : IdentityUser
    {
        public string? Address { get; set; }

        public string MainImg { get; set; } = string.Empty;

        public string CoverImg {  get; set; }=string .Empty;

        public IEnumerable<Subscribe> Subscribes { get; set; } = null!;
        public IEnumerable<ApplicationCompany> ApplicationCompanies { get; set; } = null!;

        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = null!;
    }
}
