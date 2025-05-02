namespace CompanyTraining.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }

        //public int DepartmentId { get; set; }
        //public Department Department { get; set; }
        public int QuizId { get; set; }
        public int CategoryId { get; set; }
        //public ICollection<Section> Sections { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }

    }
}
