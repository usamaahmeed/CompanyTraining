using Microsoft.EntityFrameworkCore;

namespace CompanyTraining.Models
{
    [PrimaryKey(nameof(ApplicationUserId),nameof(CourseId))]
    public class UserCourse
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public double Progress { get; set; } // e.g., 75% completion
        public int? CertificateId { get; set; }
        public Certificate Certificate { get; set; } = null!;

    }
}
