using Microsoft.AspNetCore.Identity;

namespace CompanyTraining.Models
{

    public class ApplicationUser : IdentityUser
    {

        public string? Address { get; set; }
        public string? MainImg { get; set; }
        public string? CompanyId { get; set; }
        public ApplicationUser? Company { get; set; } 
        public IEnumerable<Subscribe> Subscribes { get; set; } = null!;
        public IEnumerable<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
        public IEnumerable<UserCourse> UserCourses { get; set; } = null!;
        public IEnumerable<UserQuizAttempt> UserQuizAttempts { get; set; } = null!;
    }
}
