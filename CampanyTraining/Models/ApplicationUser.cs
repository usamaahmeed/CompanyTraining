using Microsoft.AspNetCore.Identity;

namespace CompanyTraining.Models
{
    public class ApplicationUser
    {

      public int Id { get; set; }
      public string Email { get; set; }=string.Empty;
      public string Password { get; set; }=string.Empty;
      public string ApplicationCompanyId { get; set; } = string.Empty;
      public ApplicationCompany ApplicationCompany { get; set; } = null!;
      public IEnumerable<UserCourse> UserCourses { get; set; } = null!;

      public IEnumerable<UserQuizAttempt> UserQuizAttempts { get; set; } = null!;

    }
}
