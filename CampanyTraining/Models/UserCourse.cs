namespace CompanyTraining.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsCompleted { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public double Progress { get; set; } // e.g., 75% completion

    }
}
