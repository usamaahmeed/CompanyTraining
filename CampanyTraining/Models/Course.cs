namespace CompanyTraining.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } =string.Empty;
        public bool isActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Module> Modules { get; set; } = null!;
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public ICollection<UserCourse> UserCourses { get; set; } = null!;

    }
}
