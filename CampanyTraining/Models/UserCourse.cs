namespace CompanyTraining.Models
{
    public class UserCourse
    {
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public double Progress { get; set; } // e.g., 75% completion
        public int? CertificateId { get; set; }
        public Certificate Certificate { get; set; } = null!;

    }
}
