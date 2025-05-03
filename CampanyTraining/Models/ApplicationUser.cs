using Microsoft.AspNetCore.Identity;

namespace CompanyTraining.Models
{
    public class ApplicationUser : IdentityUser
    {
      public int ApplicationCompanyId { get; set; }
      public ApplicationCompany ApplicationCompany { get; set; } = null!;

      public IEnumerable<UserCourse> UserCourses { get; set; } = null!;

    }
}
